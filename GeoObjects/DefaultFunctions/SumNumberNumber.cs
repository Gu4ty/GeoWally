using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using Compiling;
namespace GeoObjects.DefaultFunctions
{
    internal class SumNumberNumber : IInsideFunction
    {
        public SumNumberNumber(InsideFunctions functions)
        {
            Register(functions);
        }

        public int Arity
        {
            get
            {
                return 2;
            }
        }

        public string Name
        {
            get
            {
                return TokenValues.Add;
            }
        }

        public Type IsDefined(params Type[] Arguments)
        {
            if (Arguments.Length != 2)
                return null;
            Type leftOperandType = Arguments[0];
            Type rightOperandType = Arguments[1];
            if (leftOperandType == typeof(GeoNumber) && rightOperandType == typeof(GeoNumber))
                return typeof(GeoNumber);
            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            GeoNumber leftOperand = Arguments[0] as GeoNumber;
            GeoNumber rightOperand = Arguments[1] as GeoNumber;
            return new GeoNumber(leftOperand.value + rightOperand.value);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }
    }
}
