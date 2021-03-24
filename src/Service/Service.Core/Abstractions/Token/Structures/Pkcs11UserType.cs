using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Structures
{
    public enum Pkcs11UserType : long
    {
        SecurityOfficer = 0L,
        User = 1L,
        ContextSpecific = 2L
    }
}
