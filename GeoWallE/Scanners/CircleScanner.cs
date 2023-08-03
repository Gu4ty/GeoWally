using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace GeoWallE.Scanners
{
    class CircleScanner : ISpecificScanner
    {
        public CircleScanner(Scanner scanner)
        {
            Register(scanner);
        }
        public void Register(Scanner scanner)
        {
            scanner.RegisterSpecificScanner(typeof(GeoCircle), this);
        }

        public void Scan(out GeoObject input, string label)
        {
            InputForm circleInput = new InputForm("Enter Circle "+label, "X Center", "Y Center", "Radius");
            circleInput.ShowDialog();
            GeoPoint center = new GeoPoint(circleInput.input1, circleInput.input2);
            input = new GeoCircle(center, circleInput.input3);
        }
    }
}
