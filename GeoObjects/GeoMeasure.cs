using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
namespace GeoObjects
{
    public class GeoMeasure:GeoObject
    {
        public double Distance { get; }
        public GeoMeasure(double distance)
        {
            Distance = distance;
        }

        public GeoMeasure(GeoPoint p1, GeoPoint p2)
        {
            Distance = distance(p1, p2);
        }
        private double distance(GeoPoint p1, GeoPoint p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)); 
        }

    }
}
