using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using System.Reflection;
using GeoObjects.Sequences;
using GeoObjects.GeoShapes;
namespace ASTHierarchy
{
    public class DrawNode : StatementNode
    {
        string label;
        ExpressionNode expressionToDraw;

        public DrawNode(ExpressionNode ExpressionToDraw, string Label, Compiling.CodeLocation location)
        {
            label = Label;
            expressionToDraw = ExpressionToDraw;
            locationOfNode = location;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!expressionToDraw.Validate(context, defaultFunctions,errors))
                return false;
            Type expType = expressionToDraw.GetReturnedType(context, defaultFunctions,errors);
            if (expType == typeof(GeoObject))
                return true;
            if ((typeof(GeoShape).IsAssignableFrom(expType)) || (typeof(GeoSequence).IsAssignableFrom(expType)))
                return true;
            
            Compiling.CompilingError drawError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Expression can not be represented in a figure, only GeoShapes and Sequences can ");
            errors.Add(drawError);
            return false;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject objectToPaint = expressionToDraw.Run(context, defaultFunctions, manager);
            if (objectToPaint == null)
                return false;
            if (!manager.Paint(objectToPaint, context.GetCurrentColor(), label)) //Si se puede dibujar...
                manager.ThrowException(locationOfNode, "Expression can not be represented in a figure");
            return true;

        }
    }
}
