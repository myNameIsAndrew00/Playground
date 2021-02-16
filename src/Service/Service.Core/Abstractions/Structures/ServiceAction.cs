using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Structures
{ 
    public enum ServiceAction : byte
    {
        BeginSession = 0x01,
        EndSession = 0x02
    } 
}
