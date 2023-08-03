using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
using GeoObjects.Sequences;
namespace GeoObjects.DefaultFunctions
{
    class IntersectCircleCircle : IInsideFunction
    {
        public IntersectCircleCircle(InsideFunctions defaultFunctions)
        {
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
            if (leftOperandType == typeof(GeoCircle) && rightOperandType == typeof(GeoCircle))
                return typeof(GeoFiniteSequence);


            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            return intersect(Arguments[0] as GeoCircle, Arguments[1] as GeoCircle);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }


        private GeoObject SlopeDefine(GeoCircle c1, GeoCircle c2)
        {
            List<GeoObject> intersect = new List<GeoObject>();
            double x1 = c1.X;
            double y1 = c1.Y;
            double x2 = c2.X;
            double y2 = c2.Y;
            double r1 = c1.Radius;
            double r2 = c2.Radius;

            double v = (2 * x2) - (2 * x1);
            double w = (2 * y2) - (2 * y1);
            double z = (x1 * x1) + (y1 * y1) - (x2 * x2) - (y2 * y2) + (r2 * r2) - (r1 * r1);

            double a1 = ((w * w) / (v * v)) + 1;
            double a2 = ((2 * w * z) / (v * v)) + ((2 * x1 * w) / v) - (2 * y1);
            double a3 = (x1 * x1) + (y1 * y1) + ((z * z) / (v * v)) + ((2 * x1 * z) / v) - (r1 * r1);

            double discriminant = (a2 * a2) - 4 * a1 * a3;
            if (discriminant > 0)
            {
                double y0 = (((-1 * a2) + (Math.Sqrt(discriminant))) / (2 * a1));

                double y = (((-1 * a2) - (Math.Sqrt(discriminant))) / (2 * a1));

                double x0 = ((-w * y0) - z) / v;

                double x = ((-w * y) - z) / v;


                intersect.Add(new GeoPoint(x0, y0));


                intersect.Add(new GeoPoint(x, y));
            }
            else if (discriminant == 0)
            {

                double y0 = -a2 + (Math.Sqrt(discriminant) / (2 * a1));

                double x0 = ((-w * y0) - z) / v;

                intersect.Add(new GeoPoint(x0, y0));
            }
            return new GeoFiniteSequence(intersect);
        }
        private GeoObject SlopeUndefine(GeoCircle c1, GeoCircle c2)
        {
            List<GeoObject> intersect = new List<GeoObject>();

            double x1 = c1.X;
            double y1 = c1.Y;
            double x2 = c2.X;
            double y2 = c2.Y;
            double r1 = c1.Radius;
            double r2 = c2.Radius;
            double v = (2 * x2) - (2 * x1);
            double w = 0;
            double z = (x1 * x1) + (y1 * y1) - (x2 * x2) - (y2 * y2) + (r2 * r2) - (r1 * r1);

            double a1 = ((w * w) / (v * v)) + 1;
            double a2 = -2 * y1;
            double a3 = (x1 * x1) + (y1 * y1) + ((z * z) / (v * v)) + ((2 * x1 * z) / v) - (r1 * r1);

            double discriminant = (a2 * a2) - 4 * a1 * a3;
            if (discriminant > 0)
            {
                double y0 = (((-1 * a2) + (Math.Sqrt(discriminant))) / (2 * a1));

                double y = (((-1 * a2) - (Math.Sqrt(discriminant))) / (2 * a1));

                double x0 = ((-w * y0) - z) / v;

                double x = ((-w * y) - z) / v;


                intersect.Add(new GeoPoint(x0, y0));


                intersect.Add(new GeoPoint(x, y));
            }
            else if (discriminant == 0)
            {

                double y0 = -a2 + (Math.Sqrt(discriminant) / (2 * a1));

                double x0 = ((-w * y0) - z) / v;

                intersect.Add(new GeoPoint(x0, y0));
            }
            return new GeoFiniteSequence(intersect);
        }

        private GeoObject intersect(GeoCircle c1, GeoCircle c2)
        {

            return (c1.Y == c2.Y) ? SlopeUndefine(c1, c2) : SlopeDefine(c1, c2);

        }
    }
}
