using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects;
namespace ASTHierarchy
{
    public class LineConstructorNode : ExpressionNode
    {
        ExpressionNode arg1, arg2;
        Type returnedType;
        bool isValidated;
        public LineConstructorNode(ExpressionNode p1,ExpressionNode p2, Compiling.CodeLocation location)
        {
            locationOfNode = location;
            isValidated = false;
            returnedType = null;
            arg1 = p1;
            arg2 = p2;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions,errors);
            return returnedType;
        }

        public override bool Validate(IContext context,InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            if (!arg1.Validate(context,defaultFunctions,errors)|| !arg2.Validate(context,defaultFunctions,errors))
                return false;
            Type arg1Type = arg1.GetReturnedType(context, defaultFunctions,errors);
            Type arg2Type = arg2.GetReturnedType(context, defaultFunctions,errors);
            if (arg1Type == typeof(GeoObject) || arg2Type== typeof(GeoObject))
            {
                returnedType = typeof(GeoLine);
                return true;
            }

            if (arg1Type != typeof(GeoPoint) || arg2Type != typeof(GeoPoint))
            {
                Compiling.CompilingError argumentError;
                if (arg1Type != typeof(GeoPoint))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "First argument in line constructor, argument must be a GeoPoint");
                    errors.Add(argumentError);
                }
                if (arg2Type != typeof(GeoPoint))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "Second argument in line constructor, argument must be a GeoPoint");
                    errors.Add(argumentError);
                }

                return false;
            }

            returnedType = typeof(GeoLine);
            return true;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject param1 = arg1.Run(context, defaultFunctions, manager);
            GeoObject param2 = arg2.Run(context, defaultFunctions, manager);
            GeoPoint pointArgument1 = param1 as GeoPoint;
            GeoPoint pointArgument2 = param2 as GeoPoint;
            if (pointArgument1 == null || pointArgument2 == null)
            {
                if (pointArgument1 == null)
                    manager.ThrowException(locationOfNode, "Error in Line constructor,first argument wrong, type exprected: GeoPoint, Type of Argumet: " + param1.GetType().ToString());
                if (pointArgument2 == null)
                    manager.ThrowException(locationOfNode, "Error in Line constructor,second argument wrong, type exprected: GeoPoint, Type of Argumet: " + param2.GetType().ToString());
                return null;
            }
            return new GeoLine(pointArgument1, pointArgument2);
        }
    }
}
