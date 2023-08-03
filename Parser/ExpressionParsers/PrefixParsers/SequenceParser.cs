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
    class SequenceParser : IPrefixExpression
    {
        public SequenceParser(TopDownParser parser)
        {
            Register(parser);
        }
        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            List<ExpressionNode> elements = new List<ExpressionNode>();
            if (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ClosedCurlyBraces))
                return new FiniteSequenceNode(elements, token.Location);
            do
            {
                ExpressionNode arg = parser.ParseExpression();
                if (arg == null)
                {
                    CompilingError invalidExp = new CompilingError(token.Location, ErrorCode.Invalid, "Argument in sequence must be a expression");
                    parser.AddError(invalidExp);
                    return null;

                }
                elements.Add(arg);
            }
            while (parser.LookNextToken().Type==TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator));

            if (elements.Count > 1)
            {
                if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedCurlyBraces))
                {
                    CompilingError expectedClosed = new CompilingError(token.Location, ErrorCode.Expected, "ClosedCurlyBraces token");
                    parser.AddError(expectedClosed);
                    return null;
                }
                return new FiniteSequenceNode(elements, token.Location);
            }

            if(parser.LookNextToken().Type==TokenType.Symbol && parser.MatchValue(TokenValues.ClosedCurlyBraces))
            {
                return new FiniteSequenceNode(elements, token.Location);
            }

            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.PointsSuspensive))
            {
                CompilingError expectedPoints = new CompilingError(token.Location, ErrorCode.Expected, "PointSuspensive token");
                parser.AddError(expectedPoints);
                return null;
            }
            if (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ClosedCurlyBraces))
                return new InfiniteSequenceNode(elements[0], token.Location);
            ExpressionNode end = parser.ParseExpression();
            if (end == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after ... operator");
                parser.AddError(expectedExp);
                return null;
            }
            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedCurlyBraces))
            {
                CompilingError expectedClosed = new CompilingError(token.Location, ErrorCode.Expected, "ClosedCurlyBraces token");
                parser.AddError(expectedClosed);
                return null;
            }
            return new IntervalSequenceNode(elements[0], end, token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.OpenCurlyBraces, this);
        }
    }
}
