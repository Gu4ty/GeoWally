using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;
using Parser.Interfaces;
namespace Parser.ExpressionParsers.PrefixParsers
{
    /// <summary>
    /// Parsea expresiones de la forma circle(p1,m)
    /// </summary>
    class CircleConstructorParser : IPrefixExpression
    {
        public CircleConstructorParser(TopDownParser parser)
        {
            Register(parser);
        }
        public ExpressionNode Parse(TopDownParser parser, Token token)
        {                                                                                                   //Despues de ver el token circle anteriormente por el token principal, este llama al parser CircleConstructor.
            if (parser.LookNextToken().Type!=TokenType.Symbol || !parser.MatchValue(TokenValues.OpenBracket)) // Se espera entonces el token '('
            {
                CompilingError expectedOpenBracket = new CompilingError(token.Location, ErrorCode.Expected, "Expected Open Bracket token after circle keyword");
                parser.AddError(expectedOpenBracket);
                return null;
            }
            List<ExpressionNode> Arguments = new List<ExpressionNode>();
            do //Deben existir dos argumentos.
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
            while (parser.LookNextToken().Type==TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator));

            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.ClosedBracket))
            {
                CompilingError expectedClosedBracket = new CompilingError(token.Location, ErrorCode.Expected, "Expected Closed Bracket ");
                parser.AddError(expectedClosedBracket);
                return null;
            }
            if (Arguments.Count != 2) //Si no posee exactamente dos argumentos, el constructor esta mal.
            {
                CompilingError argumentError = new CompilingError(token.Location, ErrorCode.Invalid, "Invalid constructor, circle constructor expect two arguments");
                parser.AddError(argumentError);
                return null;
            }
            return new CircleConstructorNode(Arguments[0], Arguments[1],token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.Circle, this);
        }
    }
}
