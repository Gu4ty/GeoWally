using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoWallE
{
    /// <summary>
    /// Persistent drawing canvas via an image. Scaling this image might produce aliased results.
    /// Try to improve this project using your objects and managing an internal collection of drawable objects rather than an image.
    /// </summary>
    public partial class Canvas : UserControl
    {
        public Canvas()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetBackBuffer(128, 128);
            _color = Color.Black;
        }

        Graphics _gr;
        Bitmap _backBuffer;
        Color _color;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(_backBuffer, 0, 0);
        }

        private void SetBackBuffer(int width, int height)
        {
            _backBuffer = new Bitmap(width, height);
            _gr = Graphics.FromImage(_backBuffer);
            _gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            _gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _gr.PageUnit = GraphicsUnit.Pixel;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (ClientSize.Width > 0 && ClientSize.Height > 0)
                SetBackBuffer(this.ClientSize.Width, this.ClientSize.Height);
        }

        public void Clear (Color backColor)
        {
            _gr.Clear(backColor);
        }

        public void SetColor(Color color)
        {
            this._color = color;
        }

        public void DrawPoint(Point p, string label)
        {
            _gr.FillEllipse(new SolidBrush(_color), p.X - 4, p.Y - 4, 8, 8);
            if (!string.IsNullOrEmpty(label))
                _gr.DrawString(label, Font, Brushes.Black, p.X + 6, p.Y - 6);
        }

        public void DrawLine(Point p1, Point p2, string label)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            Point far1 = new Point(p2.X + dx * 100, p2.Y + dy * 100);
            Point far2 = new Point(p1.X - dx * 1, p1.Y - dy * 1);
            _gr.DrawLine(new Pen(_color), far1, far2);
            if (!string.IsNullOrEmpty(label))
                _gr.DrawString(label, Font, Brushes.Black, p1.X + 6, p1.Y - 6);
          
        }

        public void DrawCircle(Point center, int radius, string label)
        {
            _gr.DrawEllipse(new Pen(_color), center.X - radius, center.Y - radius, radius * 2, radius * 2);
            
            if (!string.IsNullOrEmpty(label))
                _gr.DrawString(label, Font, Brushes.Black, center.X + radius + 4, center.Y);
        }
    }
}
