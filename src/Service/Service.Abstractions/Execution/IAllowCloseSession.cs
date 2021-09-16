using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Execution
{
    /// <summary>
    /// Implements methods which allows to close a session object
    /// </summary>
    public interface IAllowCloseSession
    {
        /// <summary>
        /// Triggers a session to be close
        /// </summary>
        /// <param name="session">Session which must be closed</param>
        /// <returns>A boolean which represents if the session should be closed or not. If true, session will be closed.</returns>
        bool CloseSession(ISession session);
    }
}
