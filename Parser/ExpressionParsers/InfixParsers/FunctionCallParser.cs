using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;

namespace Parser.InfixParsers
{
    internal class FunctionCallParser : Interfaces.IInfixOperatorParser
    {
        public FunctionCallParser(TopDownParser parser)
        {
            Register(parser);
        }

        public int GetPrecedence()
        {
            return DefaultPrecedence.CALL;
        }

        public ExpressionNode Parse(TopDownParser parser, ExpressionNode left, Token token)
        {
            NameNode NameOfFuntion = left as NameNode;
            if (NameOfFuntion == null) return null;

            List<ExpressionNode> Arguments = new List<ExpressionNode>();
            if (parser.LookNextToken().Type==TokenType.Symbol && parser.MatchValue(TokenValues.ClosedBracket)) //Es posible que la funcion no necesite argumentos.
                return new FuncCallNode(NameOfFuntion.Identifier, token.Location, Arguments);
            
            do //Existe al menos un argumento...
            {
                ExpressionNode arg = parser.ParseExpression();
                if (arg == null)
                {
                    CompilingError invalidExp = new CompilingError(token.Location, ErrorCode.Invalid, "Invalid argument in function call, argument must be a expression");
                    parser.AddError(invalidExp);
                    return null;
                }
                Arguments.Add(arg);
            }
            while (parser.LookNextToken().Type==TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator)); //Mientra exista coma...

            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedBracket)) //Esperado ')'
            {
                CompilingError expectedClosedBracket = new CompilingError(token.Location, ErrorCode.Expected, "Closed Bracket token");
                parser.AddError(expectedClosedBracket);
               
                return null;
            }
            return new FuncCallNode(NameOfFuntion.Identifier, token.Location, Arguments);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterInfixParserByValue(TokenValues.OpenBracket, this);
        }
    }
}
