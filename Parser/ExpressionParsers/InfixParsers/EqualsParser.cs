using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;
using Parser.Interfaces;
namespace Parser.ExpressionParsers.InfixParsers
{
    class EqualsParser : IInfixOperatorParser
    {

        public EqualsParser(TopDownParser parser)
        {
            Register(parser);
        }
        public int GetPrecedence()
        {
           return DefaultPrecedence.COMPARISION;
        }

        public ExpressionNode Parse(TopDownParser parser, ExpressionNode left, Token token)
        {
            ExpressionNode right = parser.ParseExpression(GetPrecedence());
            if (right == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after == operator");
                parser.AddError(expectedExp);
                return null;
            }
            return new OperatorNode(TokenValues.Equals, token.Location,left, right);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterInfixParserByValue(TokenValues.Equals,this);
        }
    }
}
