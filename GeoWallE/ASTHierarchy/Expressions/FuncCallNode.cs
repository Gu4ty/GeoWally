using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class FuncCallNode : ExpressionNode
    {
        string Identifier;
        List<ExpressionNode> Args;
        Type returnedType;
        bool isValidated;
        public FuncCallNode(string id, Compiling.CodeLocation location, List<ExpressionNode> args)
        {
            locationOfNode = location;
            Identifier = id;
            Args = args;
            returnedType = null;
            isValidated = false;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            List<Type> argsTypes = new List<Type>();
            foreach (ExpressionNode argument in Args)
            {
                if (!argument.Validate(context, defaultFunctions, errors))
                    return false;
                argsTypes.Add(argument.GetReturnedType(context, defaultFunctions, errors));
            }

            if (defaultFunctions.IsDefined(Identifier, out returnedType, argsTypes.ToArray()))
                return true;
            if (context.IsDefined(Identifier, Args.Count))
            {
                returnedType = typeof(GeoObject);
                return true;
            }
            Compiling.CompilingError notDefinedFunct = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, "Function " + Identifier + " is not defined in this context");
            errors.Add(notDefinedFunct);
            return false;

        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions, errors);
            return returnedType;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject[] argumentsValue = new GeoObject[Args.Count];
            for (int i = 0; i < Args.Count; i++) //Hal;ando el valor de cada argumento en ejecucion...
            {
                GeoObject valueOfArgument = Args[i].Run(context, defaultFunctions, manager);
                if (valueOfArgument == null)
                    return null;
                argumentsValue[i] = valueOfArgument;
            }
            //Comprobando si la funcion esta definida anteriormente
            Tuple<string[], ExpressionNode> function = context.GetDefinedFunction(Identifier, Args.Count);
            if (function != null)
            {
                string[] argumentsOfFunction = function.Item1;
                ExpressionNode bodyOfFunction = function.Item2;
                IRunContext innerContext = context.CreateChildContext(CreateChildOptions.CopyOnlyFunctions); //Se crea un contexto interno el cual ve las demas funciones definidas, incluida ella misma(caso de recursiones).
                for (int i = 0; i < Args.Count; i++) //Definiendo los identificadores de la funcion con sus respectivos valores...
                    innerContext.Define(argumentsOfFunction[i], argumentsValue[i]);
                return bodyOfFunction.Run(innerContext, defaultFunctions, manager);
            }//sino la funcion debe ser una funcion intrinsica del lenguaje.
            GeoObject result = defaultFunctions.Operate(Identifier, argumentsValue);
            if (result == null)
            {
                string errorType = "";
                foreach (var item in argumentsValue)
                {
                    errorType += item.ToString();
                    errorType += ",  ";
                }
                manager.ThrowException(locationOfNode, "Exception in fuction call, " + Identifier + " is not defined with types: "+errorType);
            }
                return result;
        }
    }
}
