using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoObjects;

namespace ASTHierarchy
{
    public class ColorNode:StatementNode
    {
        string color;

        public ColorNode(string Color, Compiling.CodeLocation location)
        {
            color = Color;
            locationOfNode = location;
        }

        public override bool Run(IRunContext context, InsideFunctions defaultFunctions, IApplicationManager manager)
        {
            context.InsertColor(color);
            return true;
        }

        public override bool Validate(IContext context, InsideFunctions defaultFunctions, List<Compiling.CompilingError> errors)
        {
            if (isColor(color))
                return true;
            Compiling.CompilingError colorError = new Compiling.CompilingError(locationOfNode, Compiling.ErrorCode.Invalid, color + " is not a supported color");
            errors.Add(colorError);
            return false;
        }
        private bool isColor(string c)
        {
            string[] colors = "Red,Green,Blue,Cyan,Magenta,Yellow,White,Black,Gray".Split(',');
            foreach (var color in colors)
            {
                if (c == color)
                    return true;
            }
            return false;
        }


    }
}
