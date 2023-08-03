using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects.Sequences;
namespace GeoObjects.DefaultFunctions
{
    class IntersectLineLine : IInsideFunction
    {
        public IntersectLineLine(InsideFunctions defaultFunction)
        {
            Register(defaultFunction);
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
                return "intersect";
            }
        }

        public Type IsDefined(params Type[] Arguments)
        {
            if (Arguments.Length != 2)
                return null;
            Type leftOperandType = Arguments[0];
            Type rightOperandType = Arguments[1];
            if (leftOperandType == typeof(GeoLine) && rightOperandType == typeof(GeoLine))
                return typeof(GeoSequence);
            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            return intersect(Arguments[0] as GeoLine, Arguments[1] as GeoLine);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }

        private GeoObject intersect(GeoLine l1, GeoLine l2)
        {
            List<GeoObject> result = new List<GeoObject>();
            if (IsSameLine(l1, l2))
                return new Undefined();
            if (IsParallel(l1, l2))
                return new GeoFiniteSequence(result);
            double xIntersect, yIntersect;
            xIntersect = (l1.C * l2.B - l2.C * l1.B) / (l1.B * l2.A - l1.A * l2.B);
            if (l1.B > 0)
                yIntersect = -(l1.A * xIntersect + l1.C) / l1.B;
            else
                yIntersect = -(l2.A * xIntersect + l2.C) / l2.B;
            result.Add(new GeoPoint(xIntersect, yIntersect));
            return new GeoFiniteSequence(result);
        }
        private bool IsParallel(GeoLine l1, GeoLine l2)
        {
            return (l1.A == l2.A && l1.B == l2.B);
        }
        private bool IsSameLine(GeoLine l1, GeoLine l2)
        {
            return IsParallel(l1, l2) && l1.C == l2.C;
        }
    }
}
