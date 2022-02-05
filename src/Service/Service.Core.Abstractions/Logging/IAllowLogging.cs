using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Logging
{
    /// <summary>
    /// Provides methods and properties which allows this object to create log messages.
    /// </summary>
    public interface IAllowLogging
    {
        public record LogData(string Message, object Data, LogLevel LogLevel);

        public LogSection LogSection { get; }

        /// <summary>
        /// Contains logs occured during current session on this object.
        /// Logs contained by this collection will be cleared after them will be read by environment.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<LogData> Logs { get; }

        public void ClearLogs();
    }
}
