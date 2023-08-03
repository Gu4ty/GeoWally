using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    public class GeoArc:GeoCircle
    {
        public GeoPoint LimitA { get; private set; }
        public GeoPoint LimitB { get; private set; }

        public GeoArc(GeoPoint Center=null, GeoPoint limitA=null, GeoPoint limitB=null,double radius=0):base(Center,radius)
        {
            LimitA = new GeoPoint(limitA);
            LimitB = new GeoPoint(limitB);
           
        }
    }
}
