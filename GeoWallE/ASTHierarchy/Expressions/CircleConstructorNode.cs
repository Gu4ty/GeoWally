using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace ASTHierarchy
{
    public class CircleConstructorNode : ExpressionNode
    {
        ExpressionNode arg1, arg2;
        Type returnedType;
        bool isValidated;
        public CircleConstructorNode(ExpressionNode point, ExpressionNode measure, Compiling.CodeLocation location)
        {
            isValidated = false;
            returnedType = null;
            arg1 = point;
            arg2 = measure;
            locationOfNode = location;
        }
        /// <summary>
        /// Metodo que devuelve el tipo de retorno de un constructor de circle
        /// </summary>
        /// <param name="context"></param>
        /// <param name="defaultFunctions"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions, errors);
            return returnedType;
        }
        /// <summary>
        /// Metodo que valida en un contexto especifico.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="defaultFunctions"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            if (!arg1.Validate(context, defaultFunctions, errors) || !arg2.Validate(context, defaultFunctions, errors))
                return false;
            Type arg1Type = arg1.GetReturnedType(context, defaultFunctions, errors);
            Type arg2Type = arg2.GetReturnedType(context, defaultFunctions, errors);
            if (arg1Type == typeof(GeoObject) || arg2Type == typeof(GeoObject)) //Si los argumentos son genericos no se puede
            {                                                                   //decidir algo sobre los tipos.
                returnedType = typeof(GeoCircle);    //Valor de retorno actualizado.
                return true;
            }
            if (arg1Type != typeof(GeoPoint) || arg2Type != typeof(GeoMeasure)) //Ahora, si no son genericos, entonces deben
            {                                                                   //cumplir con la condicion.
                Compiling.CompilingError argumentError;
                if (arg1Type != typeof(GeoPoint))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "First argument in circle constructor , argument must be a GeoPoint");
                    errors.Add(argumentError);
                }
                if (arg2Type != typeof(GeoMeasure))
                {
                    argumentError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, "Second argument in  circle constructor, argument must be a GeoMeasure");
                    errors.Add(argumentError);
                }

                return false;
            }

            returnedType = typeof(GeoCircle); //Todo bien, se apunta que el tipo de retorno es de seguro GeoCircle.
            return true;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject param1 = arg1.Run(context, defaultFunctions, manager);
            GeoObject param2 = arg2.Run(context, defaultFunctions, manager);
            GeoPoint pointArgument = param1 as GeoPoint;
            GeoMeasure measureArgument = param2 as GeoMeasure;
            if (pointArgument == null || measureArgument == null)
            {
                if (pointArgument == null)
                    manager.ThrowException(locationOfNode, "Error in Circle constructor,first argument wrong, type exprected: GeoPoint, Type of Argumet: "+param1.GetType().ToString());
                if (measureArgument == null)
                    manager.ThrowException(locationOfNode, "Error in Circle constructor,second argument wrong, type exprected: GeoMeasure, Type of Argumet: " + param2.GetType().ToString());
                return null;
            }
            return new GeoCircle(pointArgument, measureArgument.Distance);
        }
    }
}
