using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects;
namespace ASTHierarchy
{
    public class PointInputNode : InputStatNode
    {
        public PointInputNode(string id, Compiling.CodeLocation location)
        {
            identifier = id;
            locationOfNode = location;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject point;
            manager.Scan(typeof(GeoPoint), out point, identifier);

            context.Define(identifier, point);
            return true;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (context.Define(identifier, typeof(GeoPoint)))
                return true;
            Compiling.CompilingError redefinitionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + identifier);
            errors.Add(redefinitionError);
            return false;
        }
    }
}
