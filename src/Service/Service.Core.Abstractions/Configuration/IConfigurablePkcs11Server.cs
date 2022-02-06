using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Configuration
{
    /// <summary>
    /// Implements methods which allows configuration
    /// </summary>
    public interface IConfigurablePkcs11Server
    {
        /// <summary>
        /// Get server sessions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IReadOnlySession> GetSessions();

        /// <summary>
        /// Get server logs.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ILogMessage> GetLogs();
    }
}
