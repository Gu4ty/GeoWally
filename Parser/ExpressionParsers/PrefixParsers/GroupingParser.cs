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
    internal class GroupingParser : IPrefixExpression
    {
        public GroupingParser(TopDownParser parser)
        {
            Register(parser);
        }

        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            ExpressionNode InnerExpression = parser.ParseExpression();
            if (InnerExpression == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after Open Bracket Token");
                parser.AddError(expectedExp);
                return null;
            }
            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedBracket))
            {
                CompilingError expectedClosedBracket = new CompilingError(token.Location, ErrorCode.Expected, "Closed Braclet token");
                parser.AddError(expectedClosedBracket);
                return null;
            }
            return InnerExpression;
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.OpenBracket, this);
        }
    }
}
