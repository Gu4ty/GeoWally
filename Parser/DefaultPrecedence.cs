using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    /// <summary>
    /// Define los diferentes niveles de precedencia de cada operador por defecto.
    /// En caso de necesitar un nuevo operador se debera proveer su precedencia en el metodo GetProcedence().
    /// </summary>
    internal static class DefaultPrecedence
    {
        public static  int ASSIGNMENT = 100;
        public static  int CONDITIONAL = 200;
        public static int COMPARISION = 150;
        public static  int SUM = 300;
        public static  int PRODUCT = 400;
        public static  int EXPONENT = 500;
        public static  int PREFIX = 600;
        public static  int POSTFIX = 700;
        public static  int CALL = 800;
    }
}
