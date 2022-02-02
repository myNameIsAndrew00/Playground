using Service.Core.Abstractions.Execution;
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
        /// Get server sessions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IReadOnlySession> GetSessions();
    }
}
