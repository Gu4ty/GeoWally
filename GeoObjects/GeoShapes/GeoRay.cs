using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    public class GeoRay:GeoLine
    {
        public GeoRay(GeoPoint p1=null, GeoPoint p2=null):base(p1,p2)
        {
           
        }

        public bool IsOnRay(GeoPoint p)
        {
            if (!IsOnLine(p)) return false;
            if (P2.X > P1.X)
                return p.X >= P1.X;
            else if (P2.X < P1.X)
                return p.X <= P1.X;
            else if (P2.Y > P1.Y)
                return p.Y >= P1.Y;
            else if (P2.Y < P1.Y)
                return p.Y <= P1.Y;
            return P1.Equals(p);
        }
    }
}
