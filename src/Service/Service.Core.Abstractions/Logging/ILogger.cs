using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core.Abstractions.Logging
{
    /// <summary>
    /// Provides logging functionality for a pkcs 11 server using observer pattern.
    /// </summary>
    public interface ILogger : IDisposable
    {
        public void Create(LogSection section, IEnumerable<LogData> logData);

        public void Create(LogSection section, LogData logData);

        /// <summary>
        /// Get last count messages stored by this logger.
        /// </summary>
        /// <returns>A list of messages stored</returns>
        public IEnumerable<ILogMessage> GetMessages(int count);
    }
}
