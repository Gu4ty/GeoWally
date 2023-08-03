using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace ASTHierarchy
{
    public class Context : IContext
    {
        Context parent;
        Dictionary<string, Type> variables;
        Dictionary<string, List<int>> functions;


        public Context(Context p = null)
        {
            variables = new Dictionary<string, Type>();
            functions = new Dictionary<string, List<int>>();
            parent = p;

        }

        public Context(Context parent, CreateChildOptions option)
        {
            variables = new Dictionary<string, Type>();
            functions = new Dictionary<string, List<int>>();
            if (option == CreateChildOptions.CopyDefinitions)
            {
                this.parent = parent;
                
            }
            else if (option == CreateChildOptions.CopyOnlyFunctions)
            {
                this.parent = parent;
                while (this.parent.parent != null)
                    this.parent = this.parent.parent;
                functions = new Dictionary<string, List<int>>(this.parent.functions);
                this.parent = null;
            }
            else
                parent = null;
        }



        public IContext CreateChildContext(CreateChildOptions option)
        {
            return new Context(this,option);

        }

        public bool Define(string identifier, Type type)
        {
            try
            {
                variables.Add(identifier, type);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool Define(string function, int args)
        {
            if (functions.ContainsKey(function))
            {
                foreach (var numberOfArgs in functions[function])
                {
                    if (numberOfArgs == args)
                        return false;
                }
            }

            if (parent != null && parent.IsDefined(function, args))
                return false;

            if (!functions.ContainsKey(function))
            {
                functions.Add(function, new List<int>());
                functions[function].Add(args);
            }
            else
                functions[function].Add(args);
            return true;
        }

        public bool IsDefined(string identifier)
        {
            return variables.ContainsKey(identifier) || parent != null && parent.IsDefined(identifier);
        }

        public bool IsDefined(string function, int args)
        {
            if (functions.ContainsKey(function))
            {
                foreach (var numberOfArgs in functions[function])
                {
                    if (numberOfArgs == args)
                        return true;
                }
            }
            return parent != null && parent.IsDefined(function, args);
        }

        public Type GetTypeOf(string identifier)
        {
            Type type = null;
            if(variables.TryGetValue(identifier, out type) || parent!=null &&parent.variables.TryGetValue(identifier,out type))
                return type;
            return null;
        }
    }
}
