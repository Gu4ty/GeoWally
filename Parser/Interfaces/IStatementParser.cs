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
    /// Representa un parser de instruccion general.
    /// </summary>
    public interface IStatementParser:IMiniParser
    {
        StatementNode Parse(TopDownParser parser, out int index);
        void RegisterSpecificParser(ISpecificParser parser);
    }
}
