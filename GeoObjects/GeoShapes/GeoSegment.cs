using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    public class GeoSegment:GeoLine
    {
        public GeoSegment(GeoPoint p1=null, GeoPoint p2=null):base(p1,p2)
        {

        }

        public bool IsOnSegment(GeoPoint p)
        {
            if (!IsOnLine(p))
                return false;
            return (Math.Min(P1.X, P2.X) <= p.X && p.X <= Math.Max(P1.X, P2.X)) && (Math.Min(P1.Y, P2.Y) <= p.Y && p.Y <= Math.Max(P1.Y, P2.Y));
        }
    }
}
