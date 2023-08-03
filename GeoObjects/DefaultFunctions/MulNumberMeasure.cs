using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;
namespace GeoObjects.DefaultFunctions
{
    class MulNumberMeasure : IInsideFunction
    {
        public MulNumberMeasure(InsideFunctions functions)
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
                return TokenValues.Mul;
            }
        }

        public Type IsDefined(params Type[] Arguments)
        {
            if (Arguments.Length != 2)
                return null;
            Type leftOperandType = Arguments[0];
            Type rightOperandType = Arguments[1];

            if (leftOperandType == typeof(GeoMeasure) && rightOperandType == typeof(GeoNumber))
                return typeof(GeoMeasure);
            if (leftOperandType == typeof(GeoNumber) && rightOperandType == typeof(GeoMeasure))
                return typeof(GeoMeasure);
            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            GeoObject leftOperand = Arguments[0];
            GeoObject rightOperand = Arguments[1];

            if (leftOperand is GeoMeasure && rightOperand is GeoNumber)
                return Multiple(leftOperand as GeoMeasure, rightOperand as GeoNumber);
            return Multiple(rightOperand as GeoMeasure, leftOperand as GeoNumber);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }

        private GeoMeasure Multiple(GeoMeasure m, GeoNumber n)
        {
            return new GeoMeasure(Math.Abs((int)n.value) * m.Distance);
        }
    }
}

