using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiling
{
    /// <summary>
    /// Represents an element of the language.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The value of the token. In some cases refers to the content of the token (Number, literal, comments), in other cases an enumerative value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the classification type of this token.
        /// </summary>
        public TokenType Type { get; private set; }

        /// <summary>
        /// Gets the code location of this token.
        /// </summary>
        public CodeLocation Location { get; private set; }

        /// <summary>
        /// Initializes a token
        /// </summary>
        public Token(TokenType type, string value, CodeLocation location)
        {
            this.Type = type;
            this.Value = value;
            this.Location = location;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Type, Value);
        }
    }

    public struct CodeLocation
    {
        public string File;
        public int Line;
        public int Column;
    }

    /// <summary>
    /// Represents a token type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The Token is not recognized.
        /// </summary>
        Unknwon,
        /// <summary>
        /// Current token represents a literal number
        /// </summary>
        Number,
        /// <summary>
        /// Current token represents a literal string
        /// </summary>
        Text,
        /// <summary>
        /// Current token represents a keyword
        /// </summary>
        Keyword,
        /// <summary>
        /// Current token represents an identifier
        /// </summary>
        Identifier,
        /// <summary>
        /// Current token represents an operator, grouping operator or separator
        /// </summary>
        Symbol
    }

    /// <summary>
    /// Token values represents a more abstract representation of a token.
    /// Internally handled by a simple string useful for nice switch cases management during parsing.
    /// <code>
    /// switch (token.Value)
    /// {
    ///   case TokenValues.Add: // do something with addition operation
    ///   ...
    /// }
    /// </code>
    /// </summary>
    public class TokenValues
    {
        /// <summary>
        /// This class in not intended to instanciated
        /// </summary>
        protected TokenValues() { }
        public const string PointsSuspensive = "PointsSuspensive"; //...
        public const string Add = "Addition"; // +
        public const string Sub = "Subtract"; // -
        public const string Mul = "Multiplication"; // *
        public const string Div = "Division"; // /
        public const string Mod = "Modulus"; // %
        public const string Less = "Less"; // <
        public const string LessOrEquals = "LEqual"; // <=
        public const string Greater = "Greater"; // >
        public const string GreaterOrEquals = "GEqual"; // >=
        public const string Equals = "Equals"; // ==
        public const string NotEquals = "NotEquals"; // !=
        public const string And = "And"; // and
        public const string Or = "Or"; // or
        public const string Not = "Not"; // not

        public const string Assign = "Assign"; // = 

        public const string ValueSeparator = "ValueSeparator"; // ,
        public const string StatementSeparator = "StatementSeparator"; // ;

        public const string OpenBracket = "OpenBracket"; // (
        public const string ClosedBracket = "ClosedBracket"; // )
        public const string OpenCurlyBraces = "OpenCurlyBraces"; // {
        public const string ClosedCurlyBraces = "ClosedCurlyBraces"; // }

        public const string If = "IfClausule"; // if
        public const string Then = "ThenClausule"; // then
        public const string Else = "ElseClausule"; // else

        public const string Let = "LetClausule"; // let
        public const string In = "InClausule"; // in

        public const string _ = "Underscore"; // _

        public const string Undefined = "Undefined";

        public const string EOF = "EOF";

        public const string Import = "Import";
        public const string Draw = "Draw";
        public const string Color = "Color";
        public const string Restore = "Restore";
        public const string Red = "Red";
        public const string Green = "Green";
        public const string Blue = "Blue";
        public const string Cyan = "Cyan";
        public const string Magenta = "Magenta";
        public const string Yellow = "Yellow";
        public const string White = "White";
        public const string Black = "Black";
        public const string Gray = "Gray";

        public const string Point = "Point";
        public const string Line = "Line";
        public const string Segment = "Segment";
        public const string Ray = "Ray";
        public const string Measure = "Measure";
        public const string Arc = "Arc";
        public const string Circle = "Circle";
        public const string Sequence = "Sequence";

    }
}
