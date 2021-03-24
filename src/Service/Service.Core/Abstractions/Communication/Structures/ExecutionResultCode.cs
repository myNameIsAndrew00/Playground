using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication.Structures
{
    /// <summary>
    /// Represents code returned by an executor method which process a client request
    /// </summary>
    public enum ExecutionResultCode : long
    {
        ///Code associated with CKR_OK response
        OK = 0x00000000,
        ///Code associated with CKR_GENERAL_ERROR response
        GENERAL_ERROR = 0x00000005,
        ///Code associated with CKR_USER_NOT_LOGGED_IN response
        USER_NOT_LOGGED_IN = 0x00000101,
        ///Code associated with CKR_ARGUMENTS_BAD
        ARGUMENTS_BAD = 0x00000007,
        FUNCTION_NOT_SUPPORTED = 0x00000054L
    }
}
