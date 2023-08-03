using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using Compiling;
namespace ASTHierarchy
{
    abstract  public class ASTNode
    {
        protected CodeLocation locationOfNode;
        abstract public bool Validate(IContext context,InsideFunctions defaultFunctions,List<CompilingError> errors);
    }
}
