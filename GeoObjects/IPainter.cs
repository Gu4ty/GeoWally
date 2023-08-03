using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects
{
    public interface IPainter
    {
        bool Paint(GeoObject shape, string color, string label);
    }
}
