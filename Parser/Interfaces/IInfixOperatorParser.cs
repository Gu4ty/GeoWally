using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
using ASTHierarchy;

namespace Parser.Interfaces
{
    /// <summary>
    /// Representa los parser que parsean expresiones infijas.
    /// </summary>
    public interface IInfixOperatorParser:IMiniParser
    {
        ExpressionNode Parse(TopDownParser parser, ExpressionNode left, Token token);
        int GetPrecedence();
    }
}
