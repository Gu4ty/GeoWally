using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;

namespace ASTHierarchy
{
    public class OperatorNode : ExpressionNode
    {
        string operatorId;
        bool isValidated;
        Type returnedType;
        ExpressionNode [] operands;
        public OperatorNode(string id, Compiling.CodeLocation location, params ExpressionNode [] Operands)
        {
            isValidated = false;
            returnedType = null;
            operatorId = id;
            operands = Operands;
            locationOfNode = location;
        }
        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions,errors);
            return returnedType;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject[] operandValues = new GeoObject[operands.Length];
            int i = 0;
            foreach (var op in operands)
            {
                GeoObject result = op.Run(context, defaultFunctions,manager);
                if (result == null) return null;
                operandValues[i] = result;
                i++;
            }

            GeoObject resultOp= defaultFunctions.Operate(operatorId,operandValues);
            if (resultOp == null)
            {
                string typeError = "";
                foreach (var item in operandValues)
                {
                    typeError += item.ToString();
                    typeError += ", ";
                }
                manager.ThrowException(locationOfNode, "Exception during operation, " + operatorId + " is not defined with types: "+typeError);
            }
                return resultOp;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            Type[] operandTypes = new Type[operands.Length];
            int i = 0;
            foreach (var op in operands) //Viendo el tipo de cada operando...
            {
                if (!op.Validate(context, defaultFunctions,errors))
                    return false;
                operandTypes[i] = op.GetReturnedType(context, defaultFunctions,errors);
                i++;  
            }

            if (defaultFunctions.IsDefined(operatorId, out returnedType, operandTypes)) //Si existe una funcion u operador intrinseco del lengueje con estos tipos de operadores o argumentos...
                return true;

            string types = "";
            foreach (var opTypes in operandTypes)
            {
                types += opTypes.ToString();
                types += " and ";
            }

            string argumentOfError = operatorId + " not defined with types: " + types;
            Compiling.CompilingError notDefinedError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, argumentOfError);
            errors.Add(notDefinedError);
            return false;
        }
    }
}
