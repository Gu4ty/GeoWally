using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace GeoWallE.Scanners
{
    class LineScanner : ISpecificScanner
    {
        public LineScanner(Scanner scanner)
        {
            Register(scanner);
        }
        public void Register(Scanner scanner)
        {
            scanner.RegisterSpecificScanner(typeof(GeoLine), this);
        }

        public void Scan(out GeoObject input, string label)
        {
            InputForm pointInput = new InputForm("Enter point 1 coordinates of Line "+ label, "X", "Y");
            pointInput.ShowDialog();
            GeoPoint point1 = new GeoPoint(pointInput.input1, pointInput.input2);

            pointInput = new InputForm("Enter point 2 coordinates of Line " + label, "X", "Y");
            pointInput.ShowDialog();
            GeoPoint point2 = new GeoPoint(pointInput.input1, pointInput.input2);

            input = new GeoLine(point1, point2);

        }
    }
}
