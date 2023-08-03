using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.StatementParsers;
namespace Parser.Interfaces
{
    /// <summary>
    /// Representa los parsers especificos que parsean instrucciones de tipo comandos.
    /// </summary>
    interface ICommandParser:ISpecificParser
    {
        void Register(CommandParser parser);
    }
}
