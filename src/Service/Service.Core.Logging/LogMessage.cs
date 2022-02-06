using Microsoft.Data.Sqlite;
using Service.Core.Abstractions.Logging;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Logging
{
    public class LogMessage : ILogMessage
    {
        internal LogMessage(SqliteDataReader reader)
        {
            // order is logLevel, section, message, data, timeStamp
            LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), reader.GetString(0));
            Section = (LogSection)Enum.Parse(typeof(LogSection), reader.GetString(1));
            Message = reader.GetString(2);
            Data = reader.IsDBNull(3) ? null : reader.GetString(3);
            TimeStamp = reader.GetDateTime(4);
        }

        public DateTime TimeStamp { get; }

        public LogSection Section { get; }

        public string Data { get; }

        public string Message { get; }

        public LogLevel LogLevel { get; }
    }
}
