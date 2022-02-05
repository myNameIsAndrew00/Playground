using Service.Core.Abstractions.Logging;
using Service.Core.DefinedTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core.Logging
{
    public class Logger : ILogger
    {
        public void Create(LogSection section, IEnumerable<LogData> logData)
        {
            foreach(var data in logData)
            Console.WriteLine($"[{data.LogLevel}] {section} - {data.Message} - { JsonSerializer.Serialize(data.Data)} - {DateTime.Now}");
        }

        public void Create(LogSection section, LogData logData) => Create(section, new List<LogData> { logData });
        
        public IEnumerable<ILogMessage> GetMessages(int count)
        {
            throw new NotImplementedException();
        }
    }
}