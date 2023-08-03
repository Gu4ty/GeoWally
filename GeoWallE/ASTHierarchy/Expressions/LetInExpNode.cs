using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
namespace ASTHierarchy
{
    public class LetInExpNode : ExpressionNode
    {
        List<StatementNode> LetStatements;
        ExpressionNode InExpression;
        bool isValidated;
        Type returnedType;
        public LetInExpNode(List<StatementNode> statementsLet, ExpressionNode expIn, Compiling.CodeLocation location)
        {
            isValidated = false;
            LetStatements = statementsLet;
            InExpression = expIn;
            returnedType = null;
            locationOfNode = location;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            isValidated = true;
            IContext innerContext = context.CreateChildContext(CreateChildOptions.CopyDefinitions);
            foreach (var letStat in LetStatements)
            {
                if (!letStat.Validate(innerContext, defaultFunctions,errors))
                    return false;
            }
            if (!InExpression.Validate(innerContext, defaultFunctions,errors))
                return false;
            returnedType = InExpression.GetReturnedType(innerContext, defaultFunctions,errors);
            return true;

        }

        public override Type GetReturnedType(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!isValidated)
                Validate(context, defaultFunctions,errors);
            return returnedType;
        }

        public override GeoObject Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            IRunContext LetContext = context.CreateChildContext(CreateChildOptions.CopyDefinitions);
            foreach (var stat in LetStatements)
            {
                if (!stat.Run(LetContext, defaultFunctions,manager))
                    return null;
            }
            return InExpression.Run(LetContext, defaultFunctions,manager);
        }
    }
}
