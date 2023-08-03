using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
namespace GeoObjects.DefaultFunctions
{
    class DivMeasureMeasure:IInsideFunction
    {
        public DivMeasureMeasure(InsideFunctions functions)
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
                return TokenValues.Div;
            }
        }

        public Type IsDefined(params Type[] Arguments)
        {
            if (Arguments.Length != 2)
                return null;
            Type leftOperandType = Arguments[0];
            Type rightOperandType = Arguments[1];
            if (leftOperandType == typeof(GeoMeasure) && rightOperandType == typeof(GeoMeasure))
                return typeof(GeoNumber);
            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            GeoMeasure leftOperand = Arguments[0] as GeoMeasure;
            GeoMeasure rightOperand = Arguments[1] as GeoMeasure;
            try {
                GeoNumber result = new GeoNumber((int)(leftOperand.Distance / rightOperand.Distance));
                return result;
            }
            catch
            {
                return null; // Division by 0
            }
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }
    }
}
