using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using Compiling;
using ASTHierarchy;
using Parser.StatementParsers;
namespace Parser.SpecificParsers
{
    public class PointInputParser : IInputStatementParser
    {
        public PointInputParser(InputParser parser)
        {
            Register(parser);
            
        }

        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            if (!parser.MatchValue(TokenValues.Point))
                return null;
            string identifier;
            if (parser.LookNextToken().Type != TokenType.Identifier) return null;
            identifier = parser.LookNextToken().Value;
            parser.MatchType(TokenType.Identifier);

            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.StatementSeparator))
            {
                CompilingError missingSeparator;
                if (parser.LookNextToken().Value == "EOF")
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before the end of file");
                else
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before " + parser.LookNextToken().Value);
                parser.AddError(missingSeparator);
                return null;

            }

            return new PointInputNode(identifier, token.Location);
        }

        public void Register(InputParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
