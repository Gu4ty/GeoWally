using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using Compiling;
namespace ASTHierarchy
{
    /// <summary>
    /// Representa la entidad que maneja la aplicacion en donde el lenguaje esta siendo ejecutado.
    /// </summary>
    public interface IApplicationManager
    {
        bool Paint(GeoObject shape, string color, string label);
        void Scan(Type TypeToScan,out GeoObject ToScan, string label);
        void ThrowException(CodeLocation location,string Message);
    }
}
