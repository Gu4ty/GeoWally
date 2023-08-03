using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiling
{
    /// <summary>
    /// Lexical analysis. Allows to split a raw text representing the program into the first abstract elements (tokens).<para/>
    /// i.e.
    /// if 2 &lt; 3 then 4 else 5 <para/>
    /// Can be split in tokens keyword[if] number[2] symbol[&lt;] number[3] keyword[then] number[4] keyword[else] number[5]<para/>
    /// After this process is easier to recognize if an expression refers to a conditional because it "starts with [if] keyword".
    /// </summary>
    public class LexicalAnalyzer
    {
        #region Variables

        /// <summary>
        /// Associates every operator symbol with the correspondent token value
        /// </summary>
        Dictionary<string, string> operators = new Dictionary<string, string>();
        /// <summary>
        /// Associates every keyword with the correspondent token value
        /// </summary>
        Dictionary<string, string> keywords = new Dictionary<string, string>();
        /// <summary>
        /// Associates every comment starting delimiter with their correspondent ending delimiter
        /// </summary>
        Dictionary<string, string> comments = new Dictionary<string, string>();
        /// <summary>
        /// Associates every text literal starting delimiter with their correspondent ending delimiter
        /// </summary>
        Dictionary<string, string> texts = new Dictionary<string, string>();
        /// <summary>
        /// Associates every text literal starting with a boolean indicating whenever the string literal supports multiple lines
        /// </summary>
        Dictionary<string, bool> allowLB = new Dictionary<string, bool>();

        #endregion

        #region Register methods

        /// <summary>
        /// Associates an operator symbol with the correspondent token value
        /// </summary>
        public void RegisterOperator(string op, string tokenValue)
        {
            this.operators[op] = tokenValue;
        }

        /// <summary>
        /// Associates a keyword with the correspondent token value
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tokenValue"></param>
        public void RegisterKeyword(string keyword, string tokenValue)
        {
            this.keywords[keyword] = tokenValue;

        }

        /// <summary>
        /// Associates a comment starting delimiter with its correspondent ending delimiter
        /// </summary>
        public void RegisterComment(string start, string end)
        {
            this.comments[start] = end;
        }

        /// <summary>
        /// Associates a text literal starting delimiter with their correspondent ending delimiter and the multiline support
        /// </summary>
        public void RegisterText(string start, string end, bool allowLineBreak = false)
        {
            this.texts[start] = end;
            this.allowLB[start] = allowLineBreak;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the registered keywords of this Lexical Analyzer
        /// </summary>
        public IEnumerable<string> Keywords { get { return keywords.Keys; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes an instance of a Lexical Analyzer
        /// </summary>
        public LexicalAnalyzer()
        {
        }

        #endregion

        #region Getting tokens

        /// <summary>
        /// Matches a comment part in the code and read from the stream all the comment content.
        /// The comment is discarded and errors is updated with new errors if detected.
        /// </summary>
        private bool MatchComment(TokenReader stream, List<CompilingError> errors)
        {
            foreach (var start in comments.Keys.OrderByDescending(k => k.Length))
                if (stream.Match(start))
                {
                    // comment will be discarded
                    string comment;
                    if (!stream.ReadUntil(comments[start], true, out comment))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, comments[start])); // comment ending weren't found
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Matches a new symbol in the code and read it from the string. The new symbol is added to the token list as an operator.
        /// </summary>
        private bool MatchSymbol(TokenReader stream, List<Token> tokens)
        {
            foreach (var op in operators.Keys.OrderByDescending(k => k.Length))
                if (stream.Match(op))
                {
                    tokens.Add(new Token(TokenType.Symbol, operators[op], stream.Location));
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Matches a text part in the code and read the literal from the stream.
        /// The tokens list is updated with the new string token and errors is updated with new errors if detected.
        /// </summary>
        private bool MatchText(TokenReader stream, List<Token> tokens, List<CompilingError> errors)
        {
            foreach (var start in texts.Keys.OrderByDescending(k => k.Length))
            {
                string text;
                if (stream.Match(start))
                {
                    if (!stream.ReadUntil(texts[start], allowLB[start], out text))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, texts[start]));
                    tokens.Add(new Token(TokenType.Text, text, stream.Location));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns all tokens read from the code and populate the errors list with all lexical errors detected.
        /// </summary>
        /// <param name="fileName">The name of the file used for code locations.</param>
        /// <param name="code">The string containing the code being tokenized.</param>
        /// <param name="errors">A list to add errors detected in this process.</param>
        /// <returns>An IEnumerable object with all recognized tokens.</returns>
        public IEnumerable<Token> GetTokens(string fileName, string code, List<CompilingError> errors)
        {
            List<Token> tokens = new List<Token>();

            TokenReader stream = new TokenReader(fileName, code);

            while (!stream.EOF)
            {
                string value;

                if (stream.ReadWhiteSpace())
                    continue;



                if (stream.ReadID(out value))
                {
                    if (keywords.ContainsKey(value))
                        tokens.Add(new Token(TokenType.Keyword, keywords[value], stream.Location));
                    else
                        tokens.Add(new Token(TokenType.Identifier, value, stream.Location));
                    continue;
                }



                if (stream.ReadNumber(out value))
                {
                    double d;
                    if (!double.TryParse(value, out d))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                    tokens.Add(new Token(TokenType.Number, value, stream.Location));
                    continue;
                }

                if (MatchText(stream, tokens, errors))
                    continue;

                if (MatchComment(stream, errors))
                    continue;

                if (MatchSymbol(stream, tokens))
                    continue;

                // current char is not recognized as any operator start
                var unkOp = stream.ReadAny();
                errors.Add(new CompilingError(stream.Location, ErrorCode.Unknown, unkOp.ToString()));
            }

            return tokens;
        }

        #endregion

        #region Token reader
        /// <summary>
        /// Allows to read from a string numbers, identifiers and matching some prefix.
        /// </summary>
        class TokenReader
        {
            string FileName;
            string code;
            int pos;
            int line;
            int lastLB;

            public TokenReader(string fileName, string code)
            {
                this.FileName = fileName;
                this.code = code;
                this.pos = 0;
                this.line = 1;
                this.lastLB = -1;
            }

            public CodeLocation Location
            {
                get
                {
                    return new CodeLocation
                    {
                        File = FileName,
                        Line = line,
                        Column = pos - lastLB
                    };
                }
            }

            public char Peek()
            {
                if (pos < 0 || pos >= code.Length)
                    throw new InvalidOperationException();

                return code[pos];
            }

            public bool EOF
            {
                get { return pos >= code.Length; }
            }

            public bool EOL
            {
                get { return EOF || code[pos] == '\n'; }
            }

            public bool ContinuesWith(string prefix)
            {
                if (pos + prefix.Length > code.Length)
                    return false;
                for (int i = 0; i < prefix.Length; i++)
                    if (code[pos + i] != prefix[i])
                        return false;
                return true;
            }

            public bool Match(string prefix)
            {
                if (ContinuesWith(prefix))
                {
                    pos += prefix.Length;
                    return true;
                }

                return false;
            }

            public bool ValidIdCharacter(char c, bool begining)
            {
                return c == '_' || (begining ? char.IsLetter(c) : char.IsLetterOrDigit(c));
            }

            public bool ReadID(out string id)
            {
                id = "";
                while (!EOL && ValidIdCharacter(Peek(), id.Length == 0))
                    id += ReadAny();
                return id.Length > 0;
            }

            public bool ReadNumber(out string number)
            {
                number = "";
                while (!EOL && char.IsDigit(Peek()))
                    number += ReadAny();
                
                //Editado
                int currentpos = pos;
                if (Match(".") && !EOL && !char.IsDigit(Peek()))
                {
                    pos = currentpos; return number.Length>0;
                }
                pos = currentpos;
                //Hasta aqui.

                if (!EOL  && Match("."))
                { // read decimal part
                    number += ',';
                    while (!EOL && char.IsDigit(Peek()))
                        number += ReadAny();
                }

                if (number.Length == 0)
                    return false;

                // Load number posfix, i.e., 34.0F
                // Not supported exponential formats: 1.3E+4
                while (!EOL && char.IsLetterOrDigit(Peek()))
                    number += ReadAny();

                return number.Length > 0;
            }

            public bool ReadUntil(string end, bool allowLineBreak, out string text)
            {
                text = "";
                while (!Match(end))
                {
                    if (!allowLineBreak && EOL || EOF)
                        return false;
                    text += ReadAny();
                }
                return true;
            }

            public bool ReadWhiteSpace()
            {
                if (char.IsWhiteSpace(Peek()))
                {
                    ReadAny();
                    return true;
                }
                return false;
            }

            public char ReadAny()
            {
                if (EOF)
                    throw new InvalidOperationException();

                if (EOL)
                {
                    line++;
                    lastLB = pos;
                }
                return code[pos++];
            }
        }

        #endregion
    }

}
