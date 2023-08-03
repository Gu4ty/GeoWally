using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace GeoWallE.Scanners
{
    public interface ISpecificScanner
    {
        void Scan(out GeoObject input,string label);
        void Register(Scanner scanner);
    }
}
