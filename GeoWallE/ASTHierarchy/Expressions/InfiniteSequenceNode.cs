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
    public class InfiniteSequenceNode : SequenceNode
    {
        ExpressionNode start;
        Type returnedType;
        bool isValidated;
        public InfiniteSequenceNode(ExpressionNode Start, Compiling.CodeLocation location)
        {
            locationOfNode = location;
            start = Start;
            returnedType = null;
           
            isValidated = false;
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
            if (!start.Validate(context, defaultFunctions,errors))
                return false;
            Type startType = start.GetReturnedType(context, defaultFunctions,errors);
            if (startType== typeof(GeoObject) )
            {
                returnedType = typeof(GeoInfiniteSequence);
                
                return true;
            }
            if (startType != typeof(GeoNumber))
            {
                Compiling.CompilingError sequenceError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid argument in infinite sequence, argument must be a GeoNumber ");
                errors.Add(sequenceError);
                return false;
            }
            returnedType = typeof(GeoInfiniteSequence);
            
            return true;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoNumber startObject = start.Run(context, defaultFunctions, manager) as GeoNumber;
            if (startObject == null)
            {
                manager.ThrowException(locationOfNode, "Infinite sequence must be numeric type");
                return null;
            }

            return new GeoInfiniteSequence(startObject.value);
        }

        

       
    }
}
