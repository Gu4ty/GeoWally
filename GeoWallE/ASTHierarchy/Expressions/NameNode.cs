using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class NameNode:ExpressionNode
    {
        public string Identifier {  get; private set; }

        public NameNode(string id, Compiling.CodeLocation location)
        {
            Identifier = id;
            locationOfNode = location;
        }

        public override bool Validate(IContext context,InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (context.IsDefined(this.Identifier))
                return true;
            Compiling.CompilingError notDefined = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Unknown, " Use of Unknown variable " + Identifier);
            errors.Add(notDefined);
            return false;
        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (Validate(context, defaultFunctions,errors))
                return context.GetTypeOf(Identifier);
            else
                return null;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            return context.GetDefinedVariable(Identifier);
        }
    }
}
