using Compiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWallE
{
    /// <summary>
    /// Accessing to compiling modules
    /// </summary>
    public class Compiling
    {
        private static LexicalAnalyzer __LexicalProcess;

        /// <summary>
        /// Lexical analysis. Allows to split a raw text representing the program into the first abstract elements (tokens).<para/>
        /// i.e.
        /// if 2 &lt; 3 then 4 else 5 <para/>
        /// Can be split in tokens keyword[if] number[2] symbol[&lt;] number[3] keyword[then] number[4] keyword[else] number[5]<para/>
        /// After this process is easier to recognize if an expression refers to a conditional because it "starts with [if] keyword".
        /// </summary>
        public static LexicalAnalyzer Lexical
        {
            get
            {
                if (__LexicalProcess == null)
                {
                    __LexicalProcess = new LexicalAnalyzer();

                    #region Keywords

                    __LexicalProcess.RegisterKeyword("point", TokenValues.Point);
                    __LexicalProcess.RegisterKeyword("ray", TokenValues.Ray);
                    __LexicalProcess.RegisterKeyword("segment", TokenValues.Segment);
                    __LexicalProcess.RegisterKeyword("line", TokenValues.Line);
                    __LexicalProcess.RegisterKeyword("arc", TokenValues.Arc);
                    __LexicalProcess.RegisterKeyword("circle", TokenValues.Circle);
                    __LexicalProcess.RegisterKeyword("sequence", TokenValues.Sequence);

                    __LexicalProcess.RegisterKeyword("draw", TokenValues.Draw);
                    __LexicalProcess.RegisterKeyword("if", TokenValues.If);
                    __LexicalProcess.RegisterKeyword("then", TokenValues.Then);
                    __LexicalProcess.RegisterKeyword("else", TokenValues.Else);
                    __LexicalProcess.RegisterKeyword("color", TokenValues.Color);
                    __LexicalProcess.RegisterKeyword("restore", TokenValues.Restore);
                    __LexicalProcess.RegisterKeyword("let", TokenValues.Let);
                    __LexicalProcess.RegisterKeyword("in", TokenValues.In);
                    __LexicalProcess.RegisterKeyword("import", TokenValues.Import);
                    __LexicalProcess.RegisterKeyword("and", TokenValues.And);
                    __LexicalProcess.RegisterKeyword("or", TokenValues.Or);
                    __LexicalProcess.RegisterKeyword("not", TokenValues.Not);


                    __LexicalProcess.RegisterKeyword("blue", TokenValues.Blue);
                    __LexicalProcess.RegisterKeyword("red", TokenValues.Red);
                    __LexicalProcess.RegisterKeyword("cyan", TokenValues.Cyan);
                    __LexicalProcess.RegisterKeyword("yellow", TokenValues.Yellow);
                    __LexicalProcess.RegisterKeyword("green", TokenValues.Green);
                    __LexicalProcess.RegisterKeyword("magenta", TokenValues.Magenta);
                    __LexicalProcess.RegisterKeyword("white", TokenValues.White);
                    __LexicalProcess.RegisterKeyword("black", TokenValues.Black);
                    __LexicalProcess.RegisterKeyword("gray", TokenValues.Gray);
                   
                    __LexicalProcess.RegisterKeyword("_", TokenValues._);
                    
                    __LexicalProcess.RegisterKeyword("undefined", TokenValues.Undefined);
                    #endregion

                    #region Operators
                
                    __LexicalProcess.RegisterOperator("...", TokenValues.PointsSuspensive);
                    __LexicalProcess.RegisterOperator("+", TokenValues.Add);
                    __LexicalProcess.RegisterOperator("*", TokenValues.Mul);
                    __LexicalProcess.RegisterOperator("-", TokenValues.Sub);
                    __LexicalProcess.RegisterOperator("/", TokenValues.Div);
                    __LexicalProcess.RegisterOperator("%", TokenValues.Mod);
                    __LexicalProcess.RegisterOperator("<", TokenValues.Less);
                    __LexicalProcess.RegisterOperator("<=", TokenValues.LessOrEquals);
                    __LexicalProcess.RegisterOperator(">", TokenValues.Greater);
                    __LexicalProcess.RegisterOperator(">=", TokenValues.GreaterOrEquals);
                    __LexicalProcess.RegisterOperator("==", TokenValues.Equals);
                    __LexicalProcess.RegisterOperator("!=", TokenValues.NotEquals);
                    __LexicalProcess.RegisterOperator("=", TokenValues.Assign);
                    __LexicalProcess.RegisterOperator(",", TokenValues.ValueSeparator);
                    __LexicalProcess.RegisterOperator(";", TokenValues.StatementSeparator);
                    __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket);
                    __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket);
                    __LexicalProcess.RegisterOperator("{", TokenValues.OpenCurlyBraces);
                    __LexicalProcess.RegisterOperator("}", TokenValues.ClosedCurlyBraces);


                    #endregion

                    #region Comments

                    __LexicalProcess.RegisterComment("//", "\n");
                    __LexicalProcess.RegisterComment("/*", "*/");

                    #endregion

                    #region Texts

                    __LexicalProcess.RegisterText("\"", "\"", false);

                    #endregion
                }

                return __LexicalProcess;
            }
        }
    }
}
