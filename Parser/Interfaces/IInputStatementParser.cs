using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.StatementParsers;
using ASTHierarchy;
namespace Parser.Interfaces
{
    /// <summary>
    /// Parsea instrucciones de entrada.
    /// </summary>
    public interface IInputStatementParser:ISpecificParser
    {
        void Register(InputParser parser);
    }
}
