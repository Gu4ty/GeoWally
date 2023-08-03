using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoWallE.Painters;
using System.Drawing;
using System.Reflection;
using System.IO;
using GeoObjects.Sequences;
namespace GeoWallE
{
    public class Painter : IPainter
    {
        Dictionary<Type, ISpecificPainter> painters;
        Canvas canvas;
        public Painter(Canvas c)
        {
            painters = new Dictionary<Type, ISpecificPainter>();
            canvas = c;

            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(ISpecificPainter).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
            
        }
        public bool Paint(GeoObject objectToPaint, string color, string label)
        {
            
            ISpecificPainter painter;
            if(objectToPaint is GeoFiniteSequence)
            {
                foreach (var item in (objectToPaint as GeoFiniteSequence).GetSequence() )
                {
                    if (!Paint(item, color, label))
                        return false;
                }
                return true;
            }

        
            if (painters.TryGetValue(objectToPaint.GetType(), out painter))
            {
                painter.Paint(objectToPaint, canvas, Color.FromName(color), label);
                return true;
            }
            return false;
        }


        public void RegisterSpecificPainter(Type type,ISpecificPainter painter)
        {
            painters[type] = painter;
        }
    }
}
