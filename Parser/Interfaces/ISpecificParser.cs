using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
namespace Parser.Interfaces
{
    /// <summary>
    /// Representa un parser especifico de instruccion.
    /// </summary>
    public interface ISpecificParser
    {
        StatementNode Parse(TopDownParser parser);
    }
}
