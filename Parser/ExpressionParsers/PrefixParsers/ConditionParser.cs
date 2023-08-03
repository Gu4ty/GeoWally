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
    class ConditionParser : IPrefixOperatorParser
    {

        public ConditionParser(TopDownParser parser)
        {
            Register(parser);
        }

        public int GetPrecedence()
        {
            return DefaultPrecedence.CONDITIONAL;
        }

        public ExpressionNode Parse(TopDownParser parser, Token token)
        {
            //Bastante directo: Parsea expresion -> hacer match con 'then' -> parsear expresion -> hacer match con 'else; ->parsear expresion
            ExpressionNode Condition = parser.ParseExpression();
            if (Condition == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after if keyword");
                parser.AddError(expectedExp);
                return null;
            }
            if (!parser.MatchValue(TokenValues.Then))
            {
                CompilingError expectedThen = new CompilingError(token.Location, ErrorCode.Expected, "Then keyword");
                return null;
            }
            ExpressionNode ThenExpression = parser.ParseExpression();
            if (ThenExpression == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after then keyword");
                parser.AddError(expectedExp);
                return null;
            }
            if (!parser.MatchValue(TokenValues.Else))
            {
                CompilingError expectedElse = new CompilingError(token.Location, ErrorCode.Expected, "Else keyword");
                return null;
            }
            ExpressionNode ElseExpression = parser.ParseExpression(GetPrecedence() - 1); // Restandole uno porque se asocia a la derecha.
            if (ElseExpression == null)
            {
                CompilingError expectedExp = new CompilingError(token.Location, ErrorCode.Expected, "Expression after else keyword");
                parser.AddError(expectedExp);
                return null;
            }
            return new ConditionExpNode(Condition, ThenExpression, ElseExpression, token.Location);
        }

        public void Register(TopDownParser parser)
        {
            parser.RegisterPrefixParserByValue(TokenValues.If, this);
        }
    }
}
