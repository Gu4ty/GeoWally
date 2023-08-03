using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class SimpleAssignmentNode : AssignmentNode
    {
        ExpressionNode value;
        string identifier;

        public SimpleAssignmentNode(ExpressionNode DefinedValue,string id, Compiling.CodeLocation location)
        {
            value = DefinedValue;
            locationOfNode = location;
            identifier = id;
        }
        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!value.Validate(context, defaultFunctions,errors))
                return false;
            Type valueType = value.GetReturnedType(context, defaultFunctions,errors);
            if (context.Define(identifier, valueType))
                return true;
            Compiling.CompilingError redefinitionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + identifier);
            errors.Add(redefinitionError);
            return false;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject rightValue = value.Run(context, defaultFunctions, manager);
            if (rightValue == null) return false;
            context.Define(identifier, rightValue);
            return true;
        }
    }
}
