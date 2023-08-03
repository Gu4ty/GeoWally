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
    /// Denota que es un parser especifico que parsea instrucciones que presentan el token '='
    /// </summary>
    public interface IAssignmentParser:ISpecificParser
    {
        void Register(AssignmentParser parser);     
    }
}
