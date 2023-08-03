using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects
{
    public interface IInsideFunction
    {
        int Arity { get; }
        string Name { get; }
        Type IsDefined(params Type []  Arguments);
        GeoObject Operate(params GeoObject [] Arguments);
        void Register(InsideFunctions DefaultFunctions);
    }
}
