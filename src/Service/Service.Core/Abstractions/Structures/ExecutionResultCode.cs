using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Structures
{
    /// <summary>
    /// Represents code returned by an executor method which process a client request
    /// </summary>
    public enum ExecutionResultCode : int
    {
        ///Code associated with CKR_OK response
        Ok = 0x00000000,
        ///Code associated with CKR_GENERAL_ERROR response
        ServerError = 0x00000005,
        ///Code associated with CKR_USER_NOT_LOGGED_IN response
        LogedInRequired = 0x00000101,
        ///Code associated with CKR_ARGUMENTS_BAD
        BadArguments = 0x00000007
    }
}
