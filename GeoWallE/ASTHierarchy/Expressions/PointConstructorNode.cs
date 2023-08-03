using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace ASTHierarchy
{
    public class PointConstructorNode : ExpressionNode
    {
        ExpressionNode arg1, arg2;
        Type returnedType;
        bool isValidated;
        public PointConstructorNode(ExpressionNode X, ExpressionNode Y, Compiling.CodeLocation location)
        {
            isValidated = false;
            returnedType = null;
            arg1 = X;
            arg2 = Y;
            locationOfNode = location;

        }
        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions, errors);
            return returnedType;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoNumber param1;
            GeoNumber param2;
            param1 = arg1.Run(context, defaultFunctions, manager) as GeoNumber;
            param2 = arg2.Run(context, defaultFunctions, manager) as GeoNumber;
            if (param1 == null || param2 == null)
            {
                if (param1 == null)
                {
                    manager.ThrowException(locationOfNode, "Error in Point constructor,first argument wrong");
                }
                if (param2 == null)
                    manager.ThrowException(locationOfNode, "Error in Point constructor,second argument wrong");
                return null;
            }
            return new GeoPoint(param1.value, param2.value);
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<CompilingError> errors)
        {
            isValidated = true;
            if (!arg1.Validate(context, defaultFunctions, errors) || !arg2.Validate(context, defaultFunctions, errors))
                return false;
            Type arg1Type = arg1.GetReturnedType(context, defaultFunctions, errors);
            Type arg2Type = arg2.GetReturnedType(context, defaultFunctions, errors);
            if (arg1Type == typeof(GeoObject) || arg2Type == typeof(GeoObject))
            {
                returnedType = typeof(GeoPoint);
                return true;
            }
            if (arg1Type != typeof(GeoNumber) || arg2Type != typeof(GeoNumber))
            {
                Compiling.CompilingError argumentError;
                if (arg1Type != typeof(GeoNumber))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "First argument in point constructor , argument must be a GeoNumber");
                    errors.Add(argumentError);
                }
                if (arg2Type != typeof(GeoNumber))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "Second argument in  point constructor, argument must be a GeoNumber");
                    errors.Add(argumentError);
                }

                return false;
            }

            returnedType = typeof(GeoPoint);
            return true;
        }
    }
}
