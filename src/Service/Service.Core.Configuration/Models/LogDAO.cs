using Service.Core.Abstractions.Logging;
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
            this.LogLevel = log.LogLevel.ToString();
            this.LogSection = log.Section.ToString();
            this.Data = log.Data;
        }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public string LogSection { get; set; }

        public string LogLevel { get; set; }

        public string Data { get; set; }
    }
}
