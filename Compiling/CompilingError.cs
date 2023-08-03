using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiling
{
    public class CompilingError
    {
        public CodeLocation Location { get; private set; }
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

        public CompilingError(CodeLocation location, ErrorCode code, string argument)
        {
            this.Location = location;
            this.Code = code;
            this.Argument = argument;
        }
    }

    public enum ErrorCode
    {
        None,
        Expected,
        Invalid,
        Unknown,
    }
}
