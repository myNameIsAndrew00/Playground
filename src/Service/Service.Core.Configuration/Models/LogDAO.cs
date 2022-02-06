using Service.Core.Abstractions.Logging;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration.Models
{
    public class LogDAO
    {
        public LogDAO() { }

        public LogDAO(ILogMessage log)
        {
            this.Message = log.Message;
            this.TimeStamp = log.TimeStamp;
            this.LogLevel = log.LogLevel;
            this.LogSection = log.Section;
            this.Data = log.Data;
        }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public LogSection LogSection { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Data { get; set; }
    }
}
