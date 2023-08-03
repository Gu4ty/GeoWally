using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;
using GeoObjects.GeoShapes;
namespace ASTHierarchy
{
    public class ProgramNode:ASTNode
    {
        List<StatementNode> Statements = new List<StatementNode>();
        public ProgramNode(List<StatementNode> program)
        {
            Statements = program;
        }

        public override bool Validate(IContext context,InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            bool ProgramIsOk = true;
            foreach (StatementNode s in Statements)
            {
                if (!s.Validate(context, defaultFunctions, errors))
                    ProgramIsOk = false;
            }
            return ProgramIsOk;
        }

        public bool Run(IRunContext context, InsideFunctions defaultFunctions,IApplicationManager manager)
        {
            foreach (var stat in Statements)
            {
                if (!stat.Run(context, defaultFunctions, manager))
                    return false;
            }
            return true;
        }
    }
}