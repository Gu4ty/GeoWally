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
    internal class NumberParser : IPrefixExpression
    { 

        public NumberParser(TopDownParser parser)
        {
            Register(parser);
        }

        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            return new NumberNode(token.Value, token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByType(TokenType.Number, this);
        }
    }
}
