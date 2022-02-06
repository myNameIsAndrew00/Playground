using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Models
{
    public class LogDTO
    {
        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }

        public LogSection LogSection { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Data { get; set; }
    }
}
