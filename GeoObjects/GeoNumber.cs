using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects
{
    public class GeoNumber:GeoObject
    {
        public double value { get; }
        public GeoNumber(double Value=0)
        {
            value = Value;
        }
    }
}
