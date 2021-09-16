using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.DefinedTypes
{
    public enum Pkcs11UserType : ulong
    {
        SecurityOfficer = 0,
        User = 1,
        ContextSpecific = 2
    }
}
