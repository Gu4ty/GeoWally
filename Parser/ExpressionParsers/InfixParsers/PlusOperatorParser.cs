using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;
using Parser.Interfaces;
namespace Parser.InfixParsers
{
    internal class PlusOperatorParser : IInfixOperatorParser
    {
        public PlusOperatorParser(TopDownParser parser)
        {
            Register(parser);
        }

        public int GetPrecedence()
        {
            return DefaultPrecedence.SUM;
        }

        public ExpressionNode Parse(TopDownParser parser, ExpressionNode left, Token token)
        {
            ExpressionNode right = parser.ParseExpression(GetPrecedence());
            if (right == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after + operator");
                parser.AddError(expectedExp);
                return null;
            }
            return new OperatorNode(TokenValues.Add, token.Location, left, right);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterInfixParserByValue(TokenValues.Add, this);
        }
    }
}
