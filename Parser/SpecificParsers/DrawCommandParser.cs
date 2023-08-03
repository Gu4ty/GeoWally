using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Parser.Interfaces;
using Parser.StatementParsers;
using Compiling;
namespace Parser.SpecificParsers
{
    class DrawCommandParser : ICommandParser
    {
        public DrawCommandParser(CommandParser parser)
        {
            Register(parser);
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            if (!parser.MatchValue(TokenValues.Draw))
                return null;
            ExpressionNode expressionToDraw = parser.ParseExpression();
            if (expressionToDraw == null)
            {
                CompilingError expectedExp = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Expected expression after draw token");
                parser.AddError(expectedExp);
                return null;
            }
            string label = "";
            if (parser.LookNextToken().Type == TokenType.Text)
            {
                label = parser.LookNextToken().Value;
                parser.MatchType(TokenType.Text);

            }

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

            return new DrawNode(expressionToDraw, label, token.Location);
        }

        public void Register(CommandParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
