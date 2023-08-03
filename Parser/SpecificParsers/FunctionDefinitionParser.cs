using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using ASTHierarchy;
using Parser.StatementParsers;
using Compiling;
namespace Parser.SpecificParsers
{
    /// <summary>
    /// Parsea instruccion: f(a) = a*10;
    /// </summary>
    class FunctionDefinitionParser : IAssignmentParser
    {
        public FunctionDefinitionParser(AssignmentParser parser)
        {
            Register(parser);
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            string identifier;
            if (parser.LookNextToken().Type != TokenType.Identifier) return null; //Si no es un identificador no puede ser una definicion de function.
            identifier = parser.LookNextToken().Value;//Guarda el identificador en identifier...
            parser.MatchType(TokenType.Identifier);//y avanza al siguiente token.
            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.OpenBracket)) return null; //Buscando '('

            List<string> arguments = new List<string>();
            ExpressionNode valueOfFunction;
            if (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ClosedBracket)) //A lo mejor la funcion no necesita argumentos...
            {
                if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.Assign)) //Se espera '='
                {
                    CompilingError missingAssign = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Assign token");
                    parser.AddError(missingAssign);
                    return null;
                }
                valueOfFunction = parser.ParseExpression(); //Parseando el cuerpo de la funcion...
                if (valueOfFunction == null) return null;

                if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.StatementSeparator)) //Ahora se espera ';'
                {
                    CompilingError missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator");
                    parser.AddError(missingSeparator);
                    return null;

                }

                return new FunctionDefinitionNode(identifier,token.Location, arguments, valueOfFunction);
            }

            //Bueno, la funcion tiene al menos un argumento...
            do
            {
                if (parser.LookNextToken().Type != TokenType.Identifier) //Es necesario que sea un identificador
                {
                    CompilingError missingIdentifier = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Identifier token");
                    parser.AddError(missingIdentifier);
                    return null;

                }
                arguments.Add(parser.LookNextToken().Value); //Guardalo en la lista de identificadores.
                parser.MatchType(TokenType.Identifier);
            }
            while (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator)); //Mientras exista una coma...
            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedBracket))//Despues de terminar los argumento es la hora de cerrar parentesis.
            {
                CompilingError missingClosedBracket = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Closed Bracket token");
                parser.AddError(missingClosedBracket);
                return null;
            }
            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.Assign)) //Esperado '='
            {
                CompilingError missingAssign = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Assign token");
                parser.AddError(missingAssign);
                return null;
            }
            valueOfFunction = parser.ParseExpression(); //Viendo el cuerpo de la funcion...
            if (valueOfFunction == null)
            {
                CompilingError expectedExp = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Expected expression after Assign token");
                parser.AddError(expectedExp);
                return null;
            }

            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.StatementSeparator)) //Esperando ';'
            {
                CompilingError missingSeparator;
                if (parser.LookNextToken().Value == "EOF") //Se hace una diferencia cuando se alcanza el final del archivo.
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before the end of file");
                else
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before " + parser.LookNextToken().Value);
                parser.AddError(missingSeparator);
                return null;

            }

            return new FunctionDefinitionNode(identifier,token.Location ,arguments, valueOfFunction); //Listo!!!

        }

        public void Register(AssignmentParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
