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
    public class SegmentPainter : ISpecificPainter
    {
        public SegmentPainter(Painter painter)
        {
            Register(typeof(GeoSegment), painter);
        }
        public void Paint(GeoObject shape, Canvas canvas, Color color, string label)
        {
            GeoSegment segment = shape as GeoSegment;
       

            Graphics g = canvas.CreateGraphics();
            Point extremePoint1 = new Point((int)segment.P1.X,(int) segment.P1.Y);
            Point extremePoint2 = new Point((int)segment.P2.X, (int)segment.P2.Y);
            g.DrawLine(new Pen(color), extremePoint1, extremePoint2);
            if (!string.IsNullOrEmpty(label) )
            {
                Font font = new System.Drawing.Font("Consolas", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                g.DrawString(label, font, Brushes.Black, (float)segment.P1.X - 6, (float)segment.P1.Y + 6);

            }
      
        }

        public void Register(Type type, Painter painter)
        {
            painter.RegisterSpecificPainter(type, this);
        }
    }
}
