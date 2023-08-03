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
    /// Parsea instruccion: color "keyword";
    /// </summary>
    class ColorCommandParser : ICommandParser
    {
        public ColorCommandParser(CommandParser parser)
        {
            Register(parser);
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            if (parser.LookNextToken().Type!=TokenType.Keyword || !parser.MatchValue(TokenValues.Color))
                return null;
            if (parser.LookNextToken().Type != TokenType.Keyword)
            {
                CompilingError missingKeyword = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Keyword color like blue,red...");
                parser.AddError(missingKeyword);
                return null;
            }
            string color = parser.LookNextToken().Value;
            parser.MatchType(TokenType.Keyword);

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
            return new ColorNode(color, token.Location);
        }
        
        public void Register(CommandParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
