using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    public class GeoPoint:GeoShape
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public GeoPoint()
        {
            Random r = new Random();
            X = r.Next(100,1000);
            Y = r.Next(100,1000);
        }
        public GeoPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public GeoPoint(GeoPoint p)
        {
            X = p.X;
            Y = p.Y;
        }
        public override bool Equals(object obj)
        {
            GeoPoint p = obj as GeoPoint;
            if (p == null) return false;
            return X == p.X && Y == p.Y;
        }
    }
}
