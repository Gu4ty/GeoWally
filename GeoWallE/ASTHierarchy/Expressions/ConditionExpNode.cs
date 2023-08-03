using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
using GeoObjects.Sequences;
namespace ASTHierarchy
{
    public class ConditionExpNode : ExpressionNode
    {
        ExpressionNode conditionExp, thenExp, elseExp;

        public ConditionExpNode(ExpressionNode CondExp, ExpressionNode ThenExp, ExpressionNode ElseExp, Compiling.CodeLocation location)
        {
            conditionExp = CondExp;
            thenExp = ThenExp;
            elseExp = ElseExp;
            locationOfNode = location;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!Validate(context,defaultFunctions,errors))
                return null;
            return thenExp.GetReturnedType(context,defaultFunctions,errors);
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject conditionResult = conditionExp.Run(context, defaultFunctions,manager);
            if (IsFalse(conditionResult))
                return elseExp.Run(context, defaultFunctions,manager);
            return thenExp.Run(context, defaultFunctions, manager);
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {   //Valida cada una de las expresiones: if <exp> then <exp> else <exp>
            if (!(conditionExp.Validate(context,defaultFunctions,errors) && thenExp.Validate(context,defaultFunctions,errors) && elseExp.Validate(context,defaultFunctions,errors)))
                return false;
            if (thenExp.GetReturnedType(context, defaultFunctions,errors) == typeof(GeoObject)) //Tipo generico, no se pueden chequear los tipos.
                return true;                                                               
            if (elseExp.GetReturnedType(context, defaultFunctions,errors) == typeof(GeoObject)) //Tipo generico, no se pueden chequear los tipos.
                return true;
            if (thenExp.GetReturnedType(context, defaultFunctions, errors) == elseExp.GetReturnedType(context, defaultFunctions, errors)) //Si los tipos de else y then coinciden...
                return true;
            Compiling.CompilingError matchTypesError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " If expression, then expression and else expression must be of the same type ");
            errors.Add(matchTypesError);
            return false;
        }
        /// <summary>
        /// Condiciones para que la expreison de condicoin resulte en falsa...
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        private bool IsFalse(GeoObject cond)
        {
            if(cond is GeoSequence)
            {
               return (cond as GeoSequence).IsEmpty();
            }
            if(cond is GeoNumber)
            {
                return (cond as GeoNumber).value == 0;
            }
            return cond is Undefined;
        }
    }
}
