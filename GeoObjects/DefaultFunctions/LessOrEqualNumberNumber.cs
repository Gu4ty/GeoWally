using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
namespace GeoObjects.DefaultFunctions
{
    class LessOrEqualNumberNumber:IInsideFunction
    {
        public LessOrEqualNumberNumber(InsideFunctions functions)
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
                return TokenValues.LessOrEquals;
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
            if (leftOperand.value <= rightOperand.value)
                return new GeoNumber(1);
            return new GeoNumber(0);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }


    }
}
