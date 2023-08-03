using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace GeoWallE.Painters
{
    public class PointPainter : ISpecificPainter
    {
        public PointPainter(Painter painter)
        {
            Register(typeof(GeoPoint),painter);
        }
        public void Paint(GeoObject shape, Canvas canvas, Color color, string label)
        {
            GeoPoint pointForPaint =(GeoPoint) shape;

            Graphics g = canvas.CreateGraphics();
            Point p = new Point((int)pointForPaint.X,(int)pointForPaint.Y);
            g.FillEllipse(new SolidBrush(color), p.X - 4, p.Y - 4, 8, 8);
            if (!string.IsNullOrEmpty(label)) {
                Font font = new System.Drawing.Font("Consolas", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                g.DrawString(label,font , Brushes.Black, p.X + 6, p.Y - 6);
            }
        }

     
        public void Register(Type type,Painter painter)
        {
            painter.RegisterSpecificPainter(type,this);
        }
    }
}
