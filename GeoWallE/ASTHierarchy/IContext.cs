using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTHierarchy
{
    public interface IContext
    {
        bool IsDefined(string identifier);
        bool IsDefined(string function, int args);
        bool Define(string identifier, Type type);
        bool Define(string function, int numberOfArguments);
        Type GetTypeOf(string identifier);
        IContext CreateChildContext(CreateChildOptions option);
    }

    public enum CreateChildOptions {CopyDefinitions, NoCopyDefinitions, CopyOnlyFunctions };
}
