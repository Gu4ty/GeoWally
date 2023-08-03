using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
using GeoObjects;
using GeoObjects.GeoShapes;
using GeoObjects.Sequences;
namespace ASTHierarchy
{
    public class FiniteSequenceNode : SequenceNode
    {
        List<ExpressionNode> elements;
        Type returnedType;
        bool isValidated;
        public FiniteSequenceNode(List<ExpressionNode> Elements, Compiling.CodeLocation location)
        {
            locationOfNode = location;
            elements = Elements;
            returnedType = null;
            isValidated = false;
           // Expresa la siguiente situacion:  { {1},{2}} -> esta secuencia seria de "numeros" para diferenciarla de
        }                          // la secuencia { {2},{point(10,10}} -> que esta mal.

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions, errors);
            return returnedType;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            Type typeOfSequence = null;
            foreach (var item in elements) //Se comprueba que todos los elementos de la secuencia sean del mismo tipo.
            {
                if (!item.Validate(context, defaultFunctions, errors))
                    return false;
                if (typeOfSequence == null)
                {
                    if (item.GetReturnedType(context, defaultFunctions, errors) != typeof(GeoObject))
                        typeOfSequence = item.GetReturnedType(context, defaultFunctions, errors);
                }
                else // Si TypeOfSequence esta ya definido
                {
                    if (item.GetReturnedType(context, defaultFunctions, errors) != typeof(GeoObject))//Todos los tipos distintos del generico deben
                        if (item.GetReturnedType(context, defaultFunctions, errors) != typeOfSequence) //ser igual a typeOfSequence.
                        {
                            returnedType = null;
                            Compiling.CompilingError matchTypeError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid sequence, the items in the sequence must be of the same type ");
                            errors.Add(matchTypeError);
                            return false;
                        }

                }
            }
                                          
                            
            
            returnedType = typeof(GeoFiniteSequence);
           
            return true;

        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            List<GeoObject> sequence = new List<GeoObject>();
            Type elementType = null;
            foreach (var e in elements)
            {
                GeoObject result = e.Run(context, defaultFunctions, manager);
                if (result == null) return null;
                if (elementType == null)
                {
                    if (!(result is Undefined))
                        elementType = result.GetType();
                }
                else if (!(result is Undefined) && elementType != result.GetType())
                {
                    manager.ThrowException(locationOfNode, "Exception in sequence, elements must be of the same type");
                    return null;
                }
                sequence.Add(result);
            }
            return new GeoFiniteSequence(sequence);
        }

       
    }
}
