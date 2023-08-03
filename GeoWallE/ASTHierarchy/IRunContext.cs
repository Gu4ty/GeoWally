using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public interface IRunContext
    {
        Tuple<string[], ExpressionNode> GetDefinedFunction(string function, int arguments);
        GeoObject GetDefinedVariable(string variable); 
        void Define(string variable, GeoObject value);
        void Define(string function,string [] arguments, ExpressionNode body);
        void InsertColor(string color);
        void RestoreColor();
        string GetCurrentColor();
        IRunContext CreateChildContext(CreateChildOptions option);
    }

 
}
