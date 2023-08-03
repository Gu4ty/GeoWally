using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
using ASTHierarchy;
using Parser.Interfaces;
using Parser.PrefixParsers;
using Parser.InfixParsers;
using System.Reflection;
using System.IO;
namespace Parser
{
    /// <summary>
    /// Top Down Parser que se aprovecha de la precedencia de los operadores para parsear expresiones.
    /// </summary>
    public class TopDownParser //Parser principal.
    {
        /// <summary>
        /// Propiedad que dice por donde esta el parser parseando.
        /// </summary>
        internal int NextToken { get; private set; }
        Token[] Input;
        List<CompilingError> parsingErrors;
        // Diccionarios de parser, a cada token le corresponde un parser especifico.
        //Vienen en parejas, debido  a que es necesario matchear por valor y por tipo de tokens.
        Dictionary<TokenType, IPrefixExpression> PrefixParsersByType;
        Dictionary<TokenType, IInfixOperatorParser> InfixParsersByType;
        Dictionary<string, IPrefixExpression> PrefixParsersByValue;
        Dictionary<string, IInfixOperatorParser> InfixParsersByValue;
        // Para el caso particular de los statements, no siempre se puede corresponder un token con un parser.
        // por tanto se uso una lista.
        List<IStatementParser> StatementsParsers;
        /// <summary>
        /// Recibe un IEnumerable de tokens y devuelve un programa si puede.
        /// </summary>
        /// <param name="tokens"></param>
        /// tokens para parsear.
        /// <param name="errors"></param>
        /// Errores que se producieron durante el parseo.
        /// <returns></returns>
        /// Devuelve un programa si puede, en caso de que no, devuelve null.
        public ProgramNode ParseProgram(IEnumerable<Token> tokens,out List<CompilingError> errors)
        {
            Input = tokens.ToArray();
            List<StatementNode> program = new List<StatementNode>();
            int index;
            Reset(); // Resetea las condiciones a las iniciales, como por ejemplo NextToken=0...
            
            while(NextToken < Input.Length) //Mientras queden tokens por ver...
            {
                int errorsSoFar = parsingErrors.Count; //Errores hasta ahora...

                StatementNode stat = ParserStatement(out index); //index representa el indice maximo alcanzado durante el proceso de parseo de una instruccion.
                if (stat == null)                                // Se utiliza para seguir parseando el programa una vez se haya producido un error.
                {
                    if (index == NextToken || errorsSoFar==parsingErrors.Count) // En caso de que todos los parser especificos fallen, se da error de "Invalid Statement"
                    {
                        CompilingError statementError = new CompilingError(Input[NextToken].Location, ErrorCode.Invalid, "Invalid statement starting ");
                        AddError(statementError);
                        NextToken = index + 1;
                    }
                    MoveToNextStatement(index); // Error, moverse hasta la siguiente instruccion.
                }
                else
                    program.Add(stat); //Instruccion parseada con exito, el MoveNext se actualizo con las llamadas a los parsers.
            }
            errors = parsingErrors;
            if (parsingErrors.Any()) return null;
            return new ProgramNode(program);
        }
        /// <summary>
        /// Constructor por omision.
        /// </summary>
        public TopDownParser()
        {
            NextToken = 0;
            
            StatementsParsers = new List<IStatementParser>();
            PrefixParsersByType = new Dictionary<TokenType, IPrefixExpression>();
            PrefixParsersByValue = new Dictionary<string, IPrefixExpression>();
            InfixParsersByType = new Dictionary<TokenType, IInfixOperatorParser>();
            InfixParsersByValue = new Dictionary<string, IInfixOperatorParser>();
            parsingErrors = new List<CompilingError>();
            //Buscar en el directorio por todos los posibles "MiniParsers" para añadirlos aqui.
            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IMiniParser).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
        }
        /// <summary>
        /// Metodo que parsea una instruccion.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StatementNode ParserStatement(out int index)
        {
            StatementNode stat;
            int max = NextToken;
            foreach (var StatParser in StatementsParsers) //Prueba con cada parser especifico de instruccion.
            {
                stat = StatParser.Parse(this, out index);
                if (stat != null) { return stat; }
                max = Math.Max(max, index);
            }
            index = max;
            return null;
        }
        /// <summary>
        /// Metodo que parsea una Expresion. Aprovecha el hecho que es posible encontrar para cada token un parser.
        /// </summary>
        /// <param name="precedence"></param>
        /// <returns></returns>
        public ExpressionNode ParseExpression(int precedence = 0)
        {
            if (NextToken >= Input.Length) return null;
            Token TokenForParse = Input[NextToken++];
            IPrefixExpression prefix;

            //Todas las expresiones comenzaran por una expresion prefija. Ej: numeros,variables,constructores.
            prefix = GetPrefixParser(TokenForParse); //Se espera una expresion "prefija"
            if (prefix == null)                      //, por prefija se entiende una expresion que no necesita
                return null;                         // de una expresion izquierda para parsearse

            ExpressionNode leftExpression = prefix.Parse(this, TokenForParse); // La expresion izquierda hasta ahora.
            //A partir de ahora se trata de hacer crecer la expresion izquierda para que abarque todo lo que pueda.
            while (precedence < GetPrecedence()) //Misma idea del algoritmo de Shunting Yard, pero se sustituye la pila por llamadas recursivas.
            {
                TokenForParse = Input[NextToken++];
                IInfixOperatorParser infix = GetInFixParser(TokenForParse); // Se espera una expresion "infija". Por infija se entiende toda 
                leftExpression = infix.Parse(this, leftExpression, TokenForParse); //expresion que requiera de una expresion izquierda para parsearse.
                if (leftExpression == null)
                    return null;
            }
            return leftExpression;
        }
        /// <summary>
        /// Metodo que avanza el MoveNext si el toquen que esta actualmente en la cinta concuerda en tipo con el argumento.
        /// </summary>
        /// <param name="TypeForMatch"></param>
        /// <returns></returns>
        internal bool MatchType(TokenType TypeForMatch)
        {
            if (NextToken == Input.Length) return false;

            if (Input[NextToken].Type != TypeForMatch)
                return false;
            NextToken++;
            return true;
        }
        /// <summary>
        /// /// Metodo que avanza el MoveNext si el toquen que esta actualmente en la cinta concuerda en valor con el argumento.
        /// </summary>
        /// <param name="ValueForMatch"></param>
        /// <returns></returns>
        internal bool MatchValue(string ValueForMatch)
        {
            if (NextToken == Input.Length) return false;

            if (Input[NextToken].Value != ValueForMatch)
                return false;
            NextToken++;
            return true;
        }
        /// <summary>
        /// Obtiene la precedencia del parser que le corresponderia al token que esta en la cinta actualmente.
        /// Si ya no quedan tokens o no se reconoce se devuelve la minima precedencia(-1).
        /// </summary>
        /// <returns></returns>
        private int GetPrecedence()
        {
            IInfixOperatorParser infix;
            if (NextToken == Input.Length)
                return -1;
            Token TokenForParse = Input[NextToken];
            infix = GetInFixParser(TokenForParse);
            if (infix == null)
                return -1;
            return infix.GetPrecedence();
        }

        /// <summary>
        /// Devuelve el parser prefijo asociado al token t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private IPrefixExpression GetPrefixParser(Token t)
        {
            IPrefixExpression PrefixParser;
            if (PrefixParsersByType.TryGetValue(t.Type, out PrefixParser) || PrefixParsersByValue.TryGetValue(t.Value, out PrefixParser))
                return PrefixParser;
            return null;

        }
        /// <summary>
        /// /// Devuelve el parser infijo asociado al token t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private IInfixOperatorParser GetInFixParser(Token t)
        {
            IInfixOperatorParser InPosFixParser;
            if (InfixParsersByType.TryGetValue(t.Type, out InPosFixParser) || InfixParsersByValue.TryGetValue(t.Value, out InPosFixParser))
                return InPosFixParser;
            return null;

        }
        /// <summary>
        /// Devuelve el token que esta siendo analizado en este momento.
        /// </summary>
        /// <returns></returns>
        public Token LookNextToken()
        {
            if (NextToken == Input.Length)
            {
                CodeLocation location = Input[NextToken - 1].Location;
                return new Token(TokenType.Unknwon, "EOF", location);
            }
            return Input[NextToken];
        }

        public void Reset(int InputIndex)
        {
            NextToken = InputIndex;
        }

        /***********************Funciones de Registro***********************/
        public void RegisterPrefixParserByValue(string tokenValue, IPrefixExpression parser)
        {

            PrefixParsersByValue.Add(tokenValue, parser);
        }

        public void RegisterPrefixParserByType(TokenType token, IPrefixExpression parser)
        {
            PrefixParsersByType.Add(token, parser);
        }

        public void RegisterInfixParserByValue(string tokenValue, IInfixOperatorParser parser)
        {
            InfixParsersByValue.Add(tokenValue, parser);
        }

        public void RegisterInfixParserByType(TokenType token, IInfixOperatorParser parser)
        {
            InfixParsersByType.Add(token, parser);
        }

        public void RegisterStatementParser(IStatementParser parser)
        {
            StatementsParsers.Add(parser);
        }

        /// <summary>
        /// Adiciona un error a la lista de errores.
        /// </summary>
        /// <param name="error"></param>
        public void AddError(CompilingError error)
        {
            parsingErrors.Add(error);
        }

        /* Funciones de utileria*/
        private void MoveToNextStatement(int index)
        {
            for(int i=index;i<Input.Length ;i++)
                if(Input[i].Type == TokenType.Symbol && Input[i].Value == TokenValues.StatementSeparator)
                {
                    NextToken = i+1;
                    return;
                }
            NextToken = Input.Length;
        }

        private void Reset()
        {
            NextToken = 0;
            parsingErrors.Clear();

        }
    }



}
