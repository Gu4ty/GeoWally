using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Interfaces;
using ASTHierarchy;
using Parser.StatementParsers;
using Compiling;
namespace Parser.SpecificParsers
{
    /// <summary>
    /// Parsea instruccion: a,b={1,2} o parecidas. No parsea el caso a=<exp>!!!
    /// </summary>
    class MatchDeclarationParser : IAssignmentParser
    {
        public MatchDeclarationParser(AssignmentParser parser)
        {
            Register(parser); //Cada parser especifico al construirse se debe registrar al parser al cual pertenece.
        }
        public StatementNode Parse(TopDownParser parser)
        {
            Token token = parser.LookNextToken();
            List<string> identifiers = new List<string>();
            do //Debe tener al menos un identificador.
            {
                Token lookAhead = parser.LookNextToken();
                if ((lookAhead.Type != TokenType.Identifier) && (parser.LookNextToken().Type != TokenType.Keyword || lookAhead.Value != TokenValues._))
                {
                    if (identifiers.Count == 0) //Si no he visto algun identificador hasta ahora es posible que no sea una instruccion de tipo match declaration.
                        return null;
                    //Si ya se vio al menos dos identificadores se debe esperar otro...
                    CompilingError missingIdentifier = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Identifier or Underscore token");
                    parser.AddError(missingIdentifier);
                    return null;
                }
                identifiers.Add(lookAhead.Value); //Guarda el identificador
                if (!parser.MatchType(TokenType.Identifier)) //Avanza al siguiente token...
                    parser.MatchValue(TokenValues._);
            }
            while (parser.LookNextToken().Type == TokenType.Symbol && parser.MatchValue(TokenValues.ValueSeparator)); //Mientras exista coma...
            if (identifiers.Count < 2) return null; //Si solo se tiene un identificador se deja para el SimpleAssignmentParser...
            ///Si hay >=2 identificadores...
            if (parser.LookNextToken().Type != TokenType.Symbol || !parser.MatchValue(TokenValues.Assign)) //Esperado '='
            {
                CompilingError missingAssign = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Assign token");
                parser.AddError(missingAssign);
                return null;
            }

            ExpressionNode valuesOfIdentifiers = parser.ParseExpression(); //Valor a la derecha...
            if (valuesOfIdentifiers == null)
            {
                CompilingError expectedExp = new CompilingError(parser.LookNextToken().Location, ErrorCode.Expected, "Expected expression after Assign token");
                parser.AddError(expectedExp);
                return null;

            }
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

            return new MatchDeclarationNode(identifiers, valuesOfIdentifiers, token.Location);
        }

        public void Register(AssignmentParser parser)
        {
            parser.RegisterSpecificParser(this);
        }
    }
}
