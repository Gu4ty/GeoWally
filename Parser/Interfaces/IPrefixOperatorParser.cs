using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiling;


namespace Parser.Interfaces
{
    /// <summary>
    /// Parsea expresiones prefijas que tinen asociado a ellas una precedencia.
    /// </summary>
    interface IPrefixOperatorParser:IPrefixExpression
    {
       
        int GetPrecedence();
    }
}
