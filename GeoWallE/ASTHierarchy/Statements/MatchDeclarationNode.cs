using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.Sequences;
namespace ASTHierarchy
{
    public class MatchDeclarationNode : AssignmentNode
    {
        List<string> identifiers;
        ExpressionNode RightValue;
        public MatchDeclarationNode(List<string> Ids, ExpressionNode value, Compiling.CodeLocation location)
        {
            identifiers = Ids;
            RightValue = value;
            locationOfNode = location;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            GeoObject rightValue = RightValue.Run(context, defaultFunctions, manager);
            if (rightValue == null)
                return false;
            if (rightValue is Undefined)
            {
                foreach (var id in identifiers)
                {
                    if (id != "Underscore")
                        context.Define(id, new Undefined());
                }
                return true;
            }
            GeoSequence rightSequence = rightValue as GeoSequence;
            if (rightSequence == null)
            {
                manager.ThrowException(locationOfNode, "Right side of a match declaration is not a sequence");
                return false;
            }

            int i = 0;
            foreach (var element in rightSequence.GetSequence())
            {
                if (i == identifiers.Count) return true;

                if (identifiers[i] != "Underscore")
                {
                    if (i <= (identifiers.Count - 2))
                        context.Define(identifiers[i], element);
                    else
                        context.Define(identifiers[i], rightSequence.GetSubsequence(i));
                }
                ++i;
            }
            if (i == identifiers.Count) return true;
            for (; i < identifiers.Count - 1; i++)
                if (identifiers[i] != "Underscore")
                    context.Define(identifiers[i], new Undefined());

            if (identifiers[identifiers.Count - 1] != "Underscore")
                context.Define(identifiers[identifiers.Count - 1], rightSequence.GetSubsequence(identifiers.Count - 1));

            return true;

        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (!RightValue.Validate(context, defaultFunctions, errors))
                return false;
            Type RightValueType = RightValue.GetReturnedType(context, defaultFunctions, errors);
            if (RightValueType == typeof(GeoObject))
            {
                foreach (var id in identifiers)
                {
                    if (!context.Define(id, typeof(GeoObject)))
                    {
                        Compiling.CompilingError defineError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + id);
                        errors.Add(defineError);
                    }
                }
                return true;
            }
            if (typeof(GeoSequence).IsAssignableFrom(RightValueType))
            {
                Type typeOfIdentifier = RightValue.GetReturnedType(context, defaultFunctions, errors);
                foreach (var id in identifiers)
                {
                    if (!context.Define(id, typeof(GeoObject)))
                    {
                        Compiling.CompilingError defineError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.None, " Redefinition of variable " + id);
                        errors.Add(defineError);
                    }
                }
                return true;
            }
            Compiling.CompilingError matchError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Expected, " Expected a sequence before ;");
            errors.Add(matchError);
            return false;
        }
    }
}
