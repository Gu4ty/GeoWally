using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class NumberNode:ExpressionNode
    {
        string value;

        public NumberNode(string Value, Compiling.CodeLocation location)
        {
            value = Value;
            locationOfNode = location;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            return typeof(GeoNumber);
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            return new GeoNumber(double.Parse(value));
        }

        public override bool Validate(IContext context,InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            return true;
        }
    }
}
