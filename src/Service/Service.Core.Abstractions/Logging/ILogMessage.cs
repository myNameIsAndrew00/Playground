using Service.Core.DefinedTypes;
using System;

namespace Service.Core.Abstractions.Logging
{
    /// <summary>
    /// Provides properties required by a log message.
    /// </summary>
    public interface ILogMessage
    {
        public DateTime TimeStamp { get; } 

        public LogSection Section { get; }

        public string Data { get; }
    }
}