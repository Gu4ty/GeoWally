using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class FunctionDefinitionNode : AssignmentNode
    {
        string identifier;
        List<string> arguments;
        ExpressionNode functionBody;
        public FunctionDefinitionNode(string id, Compiling.CodeLocation location, List<string> args, ExpressionNode body)
        {
            identifier = id;
            arguments = args;
            functionBody = body;
            locationOfNode = location;
        }
        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            IContext innerContext = context.CreateChildContext(CreateChildOptions.CopyOnlyFunctions);
            foreach (var argument in arguments) //Define todos los identificadores en el contexto interno como genericos. No existe inferencia de tipos en funciones.
            {
                if (!innerContext.Define(argument, typeof(GeoObject))) //Si no se puedo definir porque ya estaba definido...
                {
                    Compiling.CompilingError definedError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + argument + " in this context");
                    errors.Add(definedError);
                    return false;
                }
            }
            if(!innerContext.Define(identifier, arguments.Count)) //Si la funcion ya estaba definida en el contexto interno...
            {                                                   
                Compiling.CompilingError functionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid definition of function, " + identifier + " is already defined ");
                errors.Add(functionError);
                return false;
            }//Se define el contexto interno para soportar recursividad.
            if (!functionBody.Validate(innerContext,defaultFunctions,errors)) //Si el cuerpo de la funcion es incorrecto...
                return false;
            Type resultType;
            Type [] argumentsType = new Type[arguments.Count];
            for (int i = 0; i < argumentsType.Length; i++)
            {
                argumentsType[i] = typeof(GeoObject);
            }
            if (defaultFunctions.IsDefined(identifier, out resultType, argumentsType)) //Si la funcion ya esta definida como una funcion intrinseca del lenguaje...
            {
                Compiling.CompilingError functionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, " Invalid definition of function, " + identifier + " is a default function of GeoWalle");
                errors.Add(functionError);
                return false;
            }

            if (!context.Define(identifier, arguments.Count))
            {
                Compiling.CompilingError functionError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid," Invalid definition of function, " + identifier + " is already defined ");
                errors.Add(functionError);
                return false;
            }

            return true;
            
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            context.Define(identifier, arguments.ToArray(), functionBody);
            return true;
        }
    }
}
