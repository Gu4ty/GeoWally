using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.GeoShapes
{
    public class GeoCircle:GeoPoint
    {
        public double Radius { get; private set; }
        public GeoCircle(GeoPoint p=null, double radius=0):base(p)
        {
            if(radius == 0)
            {
                Random r = new Random();
                Radius = r.Next(100, 1000); ;
                return;
            }
            Radius = radius;
        }

    }
}
