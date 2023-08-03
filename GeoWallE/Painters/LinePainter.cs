using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
using GeoWallE;
namespace GeoWallE.Painters
{
    public class LinePainter : ISpecificPainter
    {
        public LinePainter(Painter painter)
        {
            
            Register(typeof(GeoLine),painter);
        }
        public void Paint(GeoObject shape, Canvas canvas, Color color, string label)
        {
            GeoLine line = (GeoLine)shape;
      
            Graphics g = canvas.CreateGraphics();
            Point p1 = new Point((int)line.P1.X,(int) line.P1.Y);
            Point p2 = new Point((int)line.P2.X, (int)line.P2.Y);
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            Point far1 = new Point(p2.X + dx * 3000, p2.Y + dy * 3000);
            Point far2 = new Point(p1.X - dx * 3000, p1.Y - dy * 3000);
            g.DrawLine(new Pen(color),far1,far2);
            if(!string.IsNullOrEmpty(label))
            {
                Font font = new System.Drawing.Font("Consolas", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                g.DrawString(label, font, Brushes.Black, (float)line.P1.X + 6, (float)line.P1.Y - 6);
            }
         
        }

        public void Register(Type type, Painter painter)
        {
            painter.RegisterSpecificPainter(type, this);
        }
    }
}
