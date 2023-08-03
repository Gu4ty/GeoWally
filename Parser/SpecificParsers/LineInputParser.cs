using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Parser.Interfaces;
using Compiling;
using Parser.StatementParsers;
namespace Parser.SpecificParsers
{
    public class LineInputParser : IInputStatementParser
    {
        public LineInputParser(InputParser parser)
        {
            Register(parser);
        }

        public StatementNode Parse(TopDownParser parser)
        {
            //Bastante directo, muy parecido al circle input.
            Token token = parser.LookNextToken();
            if (!parser.MatchValue(TokenValues.Line))
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
            return new LineInputNode(identifier, token.Location);
        }

        public void Register(InputParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
