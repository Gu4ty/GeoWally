using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.Sequences;
using Compiling;
using GeoObjects.GeoShapes;
namespace GeoObjects.DefaultFunctions
{
    class IntersectRayCircle:IInsideFunction
    {
        public IntersectRayCircle(InsideFunctions defaultFunctions)
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
            if (leftOperandType == typeof(GeoRay) && rightOperandType == typeof(GeoCircle))
                return typeof(GeoFiniteSequence);
            if (leftOperandType == typeof(GeoCircle) && rightOperandType == typeof(GeoRay))
                return typeof(GeoFiniteSequence);

            return null;
        }

        public GeoObject Operate(params GeoObject[] Arguments)
        {
            if (Arguments.Length != 2 || IsDefined(Arguments[0].GetType(), Arguments[1].GetType()) == null)
                return null;
            IntersectLineCircle lineCircle = new IntersectLineCircle();
            GeoObject result;
            if (Arguments[0] is GeoRay)
            {
                GeoRay rayToIntercep = Arguments[0] as GeoRay;
                GeoLine line = new GeoLine(rayToIntercep.P1, rayToIntercep.P2);
                result = lineCircle.Operate(line, Arguments[1]);
            }
            else
            {
                GeoRay rayToIntercep = Arguments[1] as GeoRay;
                GeoLine line = new GeoLine(rayToIntercep.P1, rayToIntercep.P2);
                result = lineCircle.Operate(line, Arguments[0]);
            }

            if (result is Undefined)
                return result;
            GeoFiniteSequence pointsSequence = result as GeoFiniteSequence;
            List<GeoObject> pointsToReturn = new List<GeoObject>();
            GeoRay ray = (Arguments[0] as GeoRay);
            if (ray == null) ray = Arguments[1] as GeoRay;
            foreach (var p in pointsSequence.GetSequence())
            {
                if(ray.IsOnRay(p as GeoPoint))
                {
                    pointsToReturn.Add(p);
                }

            }
            return new GeoFiniteSequence( pointsToReturn);
        }

        public void Register(InsideFunctions DefaultFunctions)
        {
            DefaultFunctions.RegisterInsideFunction(this);
        }
    }
}
