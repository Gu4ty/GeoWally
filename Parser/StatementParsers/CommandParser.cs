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
    /// Parser que agrupa a todos los parser especificos de comandos.
    /// </summary>
    class CommandParser : IStatementParser
    {
        List<ISpecificParser> commandParsers;

        public CommandParser(TopDownParser parser)
        {
            commandParsers = new List<ISpecificParser>();
            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(ICommandParser).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
            Register(parser);

        }
        public StatementNode Parse(TopDownParser parser, out int index)
        {
            int currentToken = parser.NextToken;
            StatementNode statementParsed;
            int max = currentToken;
            foreach (var SpecificInputparser in commandParsers) //Misma idea que con el AssignmentParser.
            {
                statementParsed = SpecificInputparser.Parse(parser);
                if (statementParsed != null) { index = parser.NextToken; return statementParsed; }
                max = Math.Max(max, parser.NextToken);
                parser.Reset(currentToken);
            }
            index = max;
            return null;
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterStatementParser(this);
        }

        public void RegisterSpecificParser(ISpecificParser parser)
        {
            commandParsers.Add(parser);
        }
    }
}
