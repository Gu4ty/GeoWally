using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    abstract public class StatementNode:ASTNode
    {
        abstract public bool Run(IRunContext context, InsideFunctions defaultFunctions,IApplicationManager manager);

    }
}
