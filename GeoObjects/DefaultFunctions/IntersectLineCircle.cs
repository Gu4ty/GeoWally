using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using GeoObjects.Sequences;
namespace GeoObjects.DefaultFunctions
{
    class IntersectLineCircle : IInsideFunction
    {
        public IntersectLineCircle(InsideFunctions defaultFunctions=null)
        {
            if (defaultFunctions!=null)
                Register(defaultFunctions);
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
            if (leftOperandType == typeof(GeoLine) && rightOperandType == typeof(GeoCircle))
                return typeof(GeoFiniteSequence);
            if (leftOperandType == typeof(GeoCircle) && rightOperandType == typeof(GeoLine))
                return typeof(GeoFiniteSequence);

            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            Type leftOperandType = Arguments[0].GetType();
            Type rightOperandType = Arguments[1].GetType();
            if (leftOperandType == typeof(GeoLine))
                return intersect(Arguments[0] as GeoLine, Arguments[1] as GeoCircle);
            return intersect(Arguments[1] as GeoLine, Arguments[0] as GeoCircle);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }

        private GeoObject intersect(GeoLine l1, GeoCircle c1)
        {
            List<GeoObject> result = new List<GeoObject>();

            if (l1.A == 0)
            {
                double y1 = -l1.C / l1.B;
                double a = 1;
                double b = -2 * c1.X;
                double c = c1.X * c1.X + y1 * y1 - 2 * y1 * c1.Y + c1.Y * c1.Y - c1.Radius * c1.Radius;
                double discriminant1 = b * b - 4 * a * c;
                if (discriminant1 > 0)
                {
                    double x1 = (-b + Math.Sqrt(discriminant1)) / 2 * a;
                    double x2 = (-b - Math.Sqrt(discriminant1)) / 2 * a;
                    result.Add(new GeoPoint(x1, y1));
                    result.Add(new GeoPoint(x2, y1));
                }
                else if (discriminant1 == 0)
                {
                    double x1 = (-b + Math.Sqrt(discriminant1)) / 2 * a;
                    result.Add(new GeoPoint(x1, y1));
                }
                else
                    return new Undefined();
                return new GeoFiniteSequence(result);
            }


            double x = c1.X;
            double y = c1.Y;
            double rad = c1.Radius;
            double aux1 = ((l1.B * l1.B) / (l1.A * l1.A)) + 1;
            double aux2 = (((2 * l1.B * l1.C) / (l1.A * l1.A)) + ((2 * l1.B * x) / l1.A) - 2 * y);
            double aux3 = (((l1.C * l1.C) / (l1.A * l1.A)) + ((2 * x * l1.C) / l1.A) + (x * x) + (y * y) - (rad * rad));

            double discriminant = (aux2 * aux2) - (4 * (aux1 * aux3));
            if (discriminant > 0)
            {
                double y1 = ((-aux2) - Math.Sqrt(discriminant)) / (2 * aux1);

                double y2 = ((-aux2) + Math.Sqrt(discriminant)) / (2 * aux1);


                double x1 = (-l1.B * y1 - l1.C) / l1.A;

                double x2 = (-l1.B * y2 - l1.C) / l1.A;

                result.Add(new GeoPoint(x1, y1));
                result.Add(new GeoPoint(x2, y2));
            }
            else if (discriminant == 0)
            {
                double y2 = ((-aux2) + Math.Sqrt(discriminant)) / (2 * aux1);
                double x2 = (-l1.B * y2 - l1.C) / l1.A;
                result.Add(new GeoPoint(x2, y2));
            }
            else
                return new Undefined();
            return new GeoFiniteSequence(result);
        }
    }
}
