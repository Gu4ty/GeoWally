using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
using GeoObjects;
using GeoObjects.Sequences;
namespace ASTHierarchy
{
    public class IntervalSequenceNode : SequenceNode
    {
        ExpressionNode start, end;
        Type returnedType;
        bool isValidated;
        public IntervalSequenceNode(ExpressionNode Start, ExpressionNode End, Compiling.CodeLocation location)
        {
            locationOfNode = location;
            isValidated = false;
            start = Start;
            end = End;
         
            returnedType = null;
        }
        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            if (!(start.Validate(context, defaultFunctions,errors) && end.Validate(context, defaultFunctions,errors)))
                return false;
            Type startType = start.GetReturnedType(context, defaultFunctions,errors);
            Type endType = end.GetReturnedType(context, defaultFunctions,errors);
            if (startType == typeof(GeoObject) || endType==typeof(GeoObject)  )
            {
                returnedType = typeof(GeoIntervalSequence);
                return true;
            }
            if (startType != typeof(GeoNumber) || endType != typeof(GeoNumber))
            {
                Compiling.CompilingError typeError;
                if (startType != typeof(GeoNumber))
                {
                    typeError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid first argument in a interval sequence, the argument must be a GeoNumber");
                    errors.Add(typeError);
                }
                if (endType != typeof(GeoNumber))
                {
                    typeError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid second argument in a interval sequence, the argument must be a GeoNumber");
                    errors.Add(typeError);
                }
                return false;
            }
            returnedType = typeof(GeoIntervalSequence);
      
            return true;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions,errors);
            return returnedType;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoNumber leftObject = start.Run(context, defaultFunctions, manager) as GeoNumber;
            GeoNumber righObject = end.Run(context, defaultFunctions, manager) as GeoNumber;
            if (leftObject == null)
            {
                manager.ThrowException(locationOfNode, "Exception in sequence,first elements in a interval sequence must be numeric ");
                return null;
            }
            if (righObject == null)
            {
                manager.ThrowException(locationOfNode, "Exception in sequence,second elements in a interval sequence must be numeric ");
                return null;
            }
            if (righObject.GetType() != leftObject.GetType())
            {
                manager.ThrowException(locationOfNode, "Exception in sequence,first and second elements must be of the same type ");
                return null;
            }
            return new GeoIntervalSequence(leftObject.value, righObject.value);
            
            
        }

     
    }
}
