using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects.GeoShapes;
using System.Drawing;
using GeoObjects;

namespace GeoWallE.Painters
{
    public class CirclePainter : ISpecificPainter
    {
        public CirclePainter(Painter painter)
        {
            Register(typeof(GeoCircle), painter);
        }
        public void Paint(GeoObject shape, Canvas canvas, Color color, string label)
        {
            GeoCircle circle = (GeoCircle)shape ;

            Graphics g = canvas.CreateGraphics();
            
            g.DrawEllipse(new Pen(color), (float)circle.X - (float)circle.Radius, (float)circle.Y - (float)circle.Radius, (float)circle.Radius * 2, (float)circle.Radius  * 2);
            if (!string.IsNullOrEmpty(label))
            {
                Font font = new System.Drawing.Font("Consolas", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                g.DrawString(label, font,Brushes.Black, (float)circle.X + 6,(float) circle.Y - 6);
            }
        }

        public void Register(Type type, Painter painter)
        {
            painter.RegisterSpecificPainter(type, this);
        }
    }
}
