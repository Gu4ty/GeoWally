using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;
using Parser.Interfaces;
namespace Parser.PrefixParsers
{
    class LetInParser : IPrefixOperatorParser
    {

        public LetInParser(TopDownParser parser)
        {
            Register(parser);
        }
        public int GetPrecedence()
        {
            return DefaultPrecedence.CONDITIONAL;
        }

        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            List<StatementNode> LetStatements = new List<StatementNode>();
            do
            {
                int index;

                StatementNode letStat = parser.ParserStatement(out index);

                if (letStat ==null)
                    return null;

                LetStatements.Add(letStat);
            } while (parser.LookNextToken().Type!=TokenType.Keyword || !parser.MatchValue(TokenValues.In));

            ExpressionNode inExp = parser.ParseExpression(GetPrecedence() - 1);

            if (inExp == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after in keyword");
                parser.AddError(expectedExp);
                return null;
            }

            return new LetInExpNode(LetStatements, inExp, token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.Let, this);
        }
    }
}
