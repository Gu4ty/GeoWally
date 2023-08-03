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
    class RayContructorParser : IPrefixExpression
    {

        public RayContructorParser(TopDownParser parser)
        {
            Register(parser);
        }
        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.OpenBracket))
            {
                CompilingError expectedOpenBracket = new CompilingError(token.Location, ErrorCode.Expected, "Expected Open Bracket token after ray keyword");
                parser.AddError(expectedOpenBracket);
                return null;
            }
            List<ExpressionNode> Arguments = new List<ExpressionNode>();
            do
            {
                ExpressionNode arg = parser.ParseExpression();
                if (arg == null)
                {
                    CompilingError invalidExp = new CompilingError(token.Location, ErrorCode.Invalid, "Invalid argument in constructor, argument must be a expression");
                    parser.AddError(invalidExp);
                    return null;
                }
                Arguments.Add(arg);
            }
            while (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator));

            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedBracket))
            {
                CompilingError expectedClosedBracket = new CompilingError(token.Location, ErrorCode.Expected, "Expected Closed Bracket ");
                parser.AddError(expectedClosedBracket);
                return null;
            }
            if (Arguments.Count != 2)
            {
                CompilingError argumentError = new CompilingError(token.Location, ErrorCode.Invalid, "Invalid constructor, ray constructor expect two arguments");
                parser.AddError(argumentError);
                return null;
            }
            return new RayConstructorNode(Arguments[0], Arguments[1], token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.Ray, this);
        }
    }
}
