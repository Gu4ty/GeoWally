using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using Compiling;


namespace Parser.Interfaces
{
    /// <summary>
    /// Parsea expresiones prefijas.
    /// </summary>
    public interface IPrefixExpression:IMiniParser
    {
        ExpressionNode Parse(TopDownParser parser, Token token);
    }
}
