using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Interfaces
{
    /// <summary>
    /// Representa los parsers que deben estar contenidos en el parser principal.
    /// </summary>
    public interface IMiniParser
    {
        void Register(TopDownParser parser);
    }
}
