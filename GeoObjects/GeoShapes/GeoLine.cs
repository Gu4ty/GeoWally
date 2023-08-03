using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    // Line: Ax+By+C=0
    public class GeoLine:GeoShape
    {
        public GeoPoint P1 { get; private set; }
        public GeoPoint P2 { get; private set; }
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        public GeoLine(GeoPoint p1 = null, GeoPoint p2 = null)
        {
            if (p1 == null)
            {
                P1 = new GeoPoint();
                P2 = new GeoPoint();
                return;
            }

            P1 = new GeoPoint(p1);
            P2 = new GeoPoint(p2);

            if(P1.X == P2.X)
            {
                A = 1;
                B = 0;
                C = -P1.X;
            }
            else
            {
                B = 1;
                A = -((P1.Y - P2.Y) / (P1.X - P2.X));
                C = -(A * P1.X)  - P1.Y ;
            }
        }

        public bool IsOnLine(GeoPoint p)
        {
            return (A * p.X + B * p.Y + C) == 0;
        }
    }
}
