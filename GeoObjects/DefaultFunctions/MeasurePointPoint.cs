using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
namespace GeoObjects.DefaultFunctions
{
    class MeasurePointPoint : IInsideFunction
    {
        public MeasurePointPoint(InsideFunctions functions)
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
                return "measure";
            }
        }

        public Type IsDefined(params Type[] Arguments)
        {
            if (Arguments.Length != 2)
                return null;
            Type leftOperandType = Arguments[0];
            Type rightOperandType = Arguments[1];
            if (leftOperandType == typeof(GeoPoint) && rightOperandType == typeof(GeoPoint))
                return typeof(GeoMeasure);
            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            GeoPoint leftOperand = Arguments[0] as GeoPoint;
            GeoPoint rightOperand = Arguments[1] as GeoPoint;
            return new GeoMeasure(leftOperand, rightOperand);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }
    }
}
