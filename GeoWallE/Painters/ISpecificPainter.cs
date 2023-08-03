using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using System.Drawing;
using GeoObjects.GeoShapes;
namespace GeoWallE.Painters
{
    public interface ISpecificPainter
    {
        void Register(Type type,Painter painter);
        void Paint(GeoObject shape, Canvas canvas, Color color, string label);
    }
}
