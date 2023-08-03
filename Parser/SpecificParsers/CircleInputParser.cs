using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using Compiling;
using ASTHierarchy;
using Parser.StatementParsers;
namespace Parser.SpecificParsers
{
    /// <summary>
    /// Parsea instruccion: Circle "id";
    /// </summary>
    public class CircleInputParser : IInputStatementParser
    {
        public CircleInputParser(InputParser parser)
        {
            Register(parser);
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            if (!parser.MatchValue(TokenValues.Circle)) //Si el token no es "circle" es imposible que sea una instruccion de entrada de circulo.
                return null;
            string identifier;
            if (parser.LookNextToken().Type != TokenType.Identifier) //Se espera un identificador.
                return null;

            identifier = parser.LookNextToken().Value; //Guarda el identificador...
            parser.MatchType(TokenType.Identifier);//Y avanza al siguiente token...
            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.StatementSeparator)) //Se espera ';'
            {
                CompilingError missingSeparator;
                if (parser.LookNextToken().Value == "EOF") ///EOF es un valor especial de tokens para indicar que se alcanzo el final de la cinta de tokens.
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before the end of file");
                else
                    missingSeparator = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Statement Separator before " + parser.LookNextToken().Value);
                parser.AddError(missingSeparator);
                return null;

            }
            return new CircleInputNode(identifier, token.Location);
        }

        public void Register(InputParser parser)
        {
            parser.RegisterSpecificParser( this);
        }
    }
}
