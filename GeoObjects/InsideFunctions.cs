using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
namespace GeoObjects
{
    public class InsideFunctions
    {
        List<IInsideFunction> functions;
        public InsideFunctions()
        {
            functions = new List<IInsideFunction>();
            string directory = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll")
                    continue;
                var library = Assembly.LoadFile(file);
                foreach (var type in library.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IInsideFunction).IsAssignableFrom(type))
                        Activator.CreateInstance(type, this);
                }
            }
        }

        public GeoObject Operate(string functionName, params  GeoObject[] operands)
        {
            foreach (var function in functions)
            {
                if(function.Name == functionName && function.Arity == operands.Length)
                {
                    GeoObject result = function.Operate(operands);
                    if (result != null)
                        return result;

                }
            }
            return null;
        }

        public void RegisterInsideFunction(IInsideFunction function)
        {
            functions.Add(function);
        }

        public bool IsDefined(string functionName, out Type resultType, params Type[] operands)
        {
            foreach (var type in operands)
            {
                if (type == typeof(GeoObject))
                    return searchGeneric(functionName, out resultType, operands);
            }
            return searchWithType(functionName, out resultType, operands);
        }

        private bool searchWithType(string functionName, out Type resultType, params Type[] operands)
        {
            int numberOfOperands = operands.Length;
            foreach (var function in functions)
            {
                if (function.Arity == numberOfOperands && functionName == function.Name)
                {
                    resultType = function.IsDefined(operands);
                    if (resultType != null)
                        return true;
                }
            }
            resultType = null;
            return false;
        }

        private bool searchGeneric(string functionName, out Type resultType, params Type[] operands)
        {
            int numberOfOperands = operands.Length;
            foreach (var function in functions)
            {
                if (function.Arity == numberOfOperands && functionName == function.Name)
                {
                    resultType = typeof(GeoObject);
                    return true;
                }
            }
            resultType = null;
            return false;
        }


    }
}
