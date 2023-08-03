using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using Compiling;
using System.Reflection;
using ASTHierarchy;
using System.IO;
namespace Parser.StatementParsers
{
    /// <summary>
    /// Parser que agrupa a todos los parser de entrada especificos. Ej: point a;
    /// </summary>
    public class InputParser :IStatementParser
    {
        List< ISpecificParser> InputParsers;

        public InputParser(TopDownParser parser)
        {
            InputParsers = new List<ISpecificParser>();
            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IInputStatementParser).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
            Register(parser);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterStatementParser(this);
        }



        public StatementNode Parse(TopDownParser parser, out int index)
        {
            int currentToken = parser.NextToken;
            StatementNode statementParsed;
            int max = currentToken ;
            foreach (var SpecificInputparser in InputParsers)
            {
                statementParsed = SpecificInputparser.Parse(parser);
                if (statementParsed != null) {index=parser.NextToken; return statementParsed; }
                max = Math.Max(max, parser.NextToken);
                parser.Reset(currentToken);
            }
            index = max;
            return null;
        }

        public void RegisterSpecificParser( ISpecificParser parser)
        {
            InputParsers.Add(parser);
        }

     
    }
}
