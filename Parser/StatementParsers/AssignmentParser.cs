using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Parser.Interfaces;
using System.Reflection;
using System.IO;
namespace Parser.StatementParsers
{
    /// <summary>
    /// Parser que agrupa a todos los parser especificos que matchean con el signo =.
    /// </summary>
    public class AssignmentParser : IStatementParser
    {
        List<ISpecificParser> assignmentParsers;
        public AssignmentParser(TopDownParser parser)
        {
            assignmentParsers = new List<ISpecificParser>();
            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IAssignmentParser).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
            Register(parser); // Cada parser se registra a un parser que lo contenga.
        }

        public StatementNode Parse(TopDownParser parser, out int index)
        {
            int currentToken = parser.NextToken;
            StatementNode statementParsed;
            int max = -1; // Cada parser de instruccion que agrupa a otros parser tienen la capacidad de informar cual fue el maximo de avance en la cinta para seguir parseando a partir de ahi.
            foreach (var specificAssignmentParser in assignmentParsers) //Trata con todos los posibles parsers especificos.
            {
                statementParsed = specificAssignmentParser.Parse(parser);
                if (statementParsed != null) { index = parser.NextToken; return statementParsed; }
                max = Math.Max(max, parser.NextToken); //Actualiza el maximo adelantado.
                parser.Reset(currentToken);
            }
            index = max;
            return null;
        }

        /// <summary>
        /// Metodo para que este parser se registre en el parser principal.
        /// </summary>
        /// <param name="parser"></param>
        public void Register(TopDownParser parser)
        {
            parser.RegisterStatementParser(this);
        }
        /// <summary>
        /// Metodo para registrar un parser especifico respecto al signo =. Ej: a=3;  f(a)=a*10; ....
        /// </summary>
        /// <param name="parser"></param>
        public void RegisterSpecificParser(ISpecificParser parser)
        {
            assignmentParsers.Add(parser);
        }
    }
}
