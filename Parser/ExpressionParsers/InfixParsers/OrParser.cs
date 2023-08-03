using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using Compiling;
using ASTHierarchy;

namespace Parser.ExpressionParsers.InfixParsers
{
    class OrParser : IInfixOperatorParser
    {
        public OrParser(TopDownParser parser)
        {
            Register(parser);
        }
        public int GetPrecedence()
        {
            return DefaultPrecedence.COMPARISION-10;
        }

        public ExpressionNode Parse(TopDownParser parser, ExpressionNode left, Token token)
        {
            ExpressionNode right = parser.ParseExpression(GetPrecedence());
            if (right == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after or operator");
                parser.AddError(expectedExp);
                return null;
            }
            return new OperatorNode(TokenValues.Or, token.Location, left, right);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterInfixParserByValue(TokenValues.Or, this);
        }
    }
}