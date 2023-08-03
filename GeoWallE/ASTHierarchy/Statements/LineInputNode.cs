using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects;
namespace ASTHierarchy
{
    public class LineInputNode : InputStatNode
    {
        public LineInputNode(string id, Compiling.CodeLocation location)
        {
            locationOfNode = location;
            identifier = id;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject line;
            manager.Scan(typeof(GeoLine), out line, identifier);

            context.Define(identifier, line);
            return true;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (context.Define(identifier, typeof(GeoCircle)))
                return true;
            Compiling.CompilingError redefinitionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + identifier);
            errors.Add(redefinitionError);
            return false;
        }
    }
}
