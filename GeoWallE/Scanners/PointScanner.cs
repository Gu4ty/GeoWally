using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace GeoWallE.Scanners
{
    public class PointScanner : ISpecificScanner
    {
        public PointScanner(Scanner scanner)
        {
            Register(scanner);
        }
        public void Register(Scanner scanner)
        {
            scanner.RegisterSpecificScanner(typeof(GeoPoint), this);
        }

        public void Scan(out GeoObject input, string label)
        {
            InputForm pointInput = new InputForm("Enter coordinates of Point "+label, "X", "Y");
            pointInput.ShowDialog();
            GeoPoint point = new GeoPoint(pointInput.input1, pointInput.input2);
            input = point;
        }
    }
}
