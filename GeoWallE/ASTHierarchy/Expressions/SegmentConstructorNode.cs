using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects;
namespace ASTHierarchy
{
    public class SegmentConstructorNode : ExpressionNode
    {
        ExpressionNode arg1, arg2;
        Type returnedType;
        bool isValidated;
        public SegmentConstructorNode(ExpressionNode p1, ExpressionNode p2, Compiling.CodeLocation location)
        {
            isValidated = false;
            returnedType = null;
            arg1 = p1;
            locationOfNode = location;
            arg2 = p2;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions,errors);
            return returnedType;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            if (!arg1.Validate(context, defaultFunctions,errors) || !arg2.Validate(context, defaultFunctions,errors))
                return false;
            Type arg1Type = arg1.GetReturnedType(context, defaultFunctions,errors);
            Type arg2Type = arg2.GetReturnedType(context, defaultFunctions,errors);
            if (arg1Type == typeof(GeoObject) || arg2Type == typeof(GeoObject))
            {
                returnedType = typeof(GeoSegment);
                return true;
            }

            if (arg1Type != typeof(GeoPoint) || arg2Type != typeof(GeoPoint))
            {
                Compiling.CompilingError argumentError;
                if (arg1Type != typeof(GeoPoint))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "First argument in segment constructor, argument must be a GeoPoint");
                    errors.Add(argumentError);
                }
                if (arg2Type != typeof(GeoPoint))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "Second argument in segment constructor, argument must be a GeoPoint");
                    errors.Add(argumentError);
                }

                return false;
            }
            returnedType = typeof(GeoSegment);
            return true;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoPoint param1, param2;
            param1 = arg1.Run(context, defaultFunctions, manager) as GeoPoint;
            param2 = arg2.Run(context, defaultFunctions, manager) as GeoPoint;
            if (param1 == null || param2 == null)
            {
                if (param1 == null)
                    manager.ThrowException(locationOfNode, "Error in Segment constructor,first argument wrong");
                if (param2 == null)
                    manager.ThrowException(locationOfNode, "Error in Segment constructor,second argument wrong");
                return null;
            }
                return new GeoSegment(param1, param2);
        }
    }
}
