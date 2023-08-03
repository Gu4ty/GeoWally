using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoWallE.Scanners;
using System.IO;
using System.Reflection;
using GeoObjects;
namespace GeoWallE
{
    public class Scanner
    {

        Dictionary<Type, ISpecificScanner> scannerByType;

        public Scanner()
        {
            scannerByType = new Dictionary<Type, ISpecificScanner>();

            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(ISpecificScanner).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }

        }

        public void Scan(Type typeToScan,out GeoObject objectToScan, string label)
        {
            ISpecificScanner specificScanner = scannerByType[typeToScan];
            specificScanner.Scan(out objectToScan,label);
        }

        public void RegisterSpecificScanner(Type type, ISpecificScanner scanner)
        {
            scannerByType.Add(type, scanner);
        }


    }
}
