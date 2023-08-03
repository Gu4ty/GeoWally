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
    /// <summary>
    /// Parsea el caso especial a=<exp>;
    /// </summary>
    class SimpleAssignmentParser : IAssignmentParser
    {
        public SimpleAssignmentParser(AssignmentParser parser)
        {
            Register(parser);
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            string identifier;
            if (parser.LookNextToken().Type != TokenType.Identifier) return null;

            identifier = parser.LookNextToken().Value;
            parser.MatchType(TokenType.Identifier);

            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.Assign)) return null;

            ExpressionNode valueOfIdentifier = parser.ParseExpression();
            if (valueOfIdentifier == null)
            {
                CompilingError expectedExp = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Expected expression after Assign token");
                parser.AddError(expectedExp);
                return null;
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

            return new SimpleAssignmentNode(valueOfIdentifier, identifier, token.Location);
        }

        public void Register(AssignmentParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
