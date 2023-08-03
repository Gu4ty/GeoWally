using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    abstract public class ExpressionNode:ASTNode
    {
        public abstract Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors);
        abstract public GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager);
    }   
}
