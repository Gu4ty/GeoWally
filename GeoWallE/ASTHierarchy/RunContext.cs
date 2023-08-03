using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class RunContext : IRunContext
    {
        Dictionary<string, GeoObject> variables;
        Dictionary<string, List<Tuple<string[], ExpressionNode>>> functions;
        Stack<string> colors;

        RunContext parent;

        public RunContext()
        {
            variables = new Dictionary<string, GeoObject>();
            functions = new Dictionary<string, List<Tuple<string[], ExpressionNode>>>();
            colors = new Stack<string>();
            colors.Push("Black");
            parent = null;
        }

        public RunContext(RunContext p, CreateChildOptions option)
        {

            variables = new Dictionary<string, GeoObject>();
            functions = new Dictionary<string, List<Tuple<string[], ExpressionNode>>>();
            colors = new Stack<string>();
            colors.Push("Black");

            if (option == CreateChildOptions.CopyDefinitions)
            {

                parent = p;
              

            }
            else if (option == CreateChildOptions.CopyOnlyFunctions)
            {
                parent = p;
                while (parent.parent != null)
                    parent = parent.parent;
                functions = new Dictionary<string, List<Tuple<string[], ExpressionNode>>>(parent.functions);
                parent = null;
            }
            else
                parent = null;


        }

        public void Define(string variable, GeoObject value)
        {
            variables.Add(variable, value);
        }
        public void Define(string function, string[] arguments, ExpressionNode body)
        {
            if (functions.ContainsKey(function))
            {
                functions[function].Add(new Tuple<string[], ExpressionNode>(arguments, body));
            }
            else
            {
                functions.Add(function, new List<Tuple<string[], ExpressionNode>>());
                functions[function].Add(new Tuple<string[], ExpressionNode>(arguments, body));
            }
        }


        public void InsertColor(string color)
        {
            colors.Push(color);
        }
        public void RestoreColor()
        {
            if (colors.Count > 1)
                colors.Pop();
        }
        public string GetCurrentColor()
        {
            return colors.Peek();
        }

        public IRunContext CreateChildContext(CreateChildOptions option)
        {
            return new RunContext(this, option);
        }

        public Tuple<string[], ExpressionNode> GetDefinedFunction(string function, int args)
        {
            List<Tuple<string[], ExpressionNode>> f;
            if (!functions.TryGetValue(function, out f))
            {
                if (parent != null)
                {
                    return parent.GetDefinedFunction(function, args);
                }

                return null;

            }
            foreach (var tuple in f)
            {
                if (tuple.Item1.Length == args)
                    return tuple;
            }
            return null;
        }

        public GeoObject GetDefinedVariable(string variable)
        {
            try
            {
                return variables[variable];
            }
            catch
            {
                if(parent!=null)
                    return parent.GetDefinedVariable(variable);
                return null;
            }
        }
    }
}
