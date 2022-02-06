using Service.ConfigurationAPI.Models;
using Service.Core.DefinedTypes;

namespace Configurator.ViewModel.Pages.Logging
{
    public class LogsItemViewModel : BaseViewModel
    {
        public LogsItemViewModel(LogDTO log)
        {
            this.LogLevel = log.LogLevel;
            this.Message = log.Message;
            this.TimeStamp = log.TimeStamp.ToString(DATETIME_FORMAT);
            this.Section = log.LogSection;
        }

        public LogLevel LogLevel { get; set; }

        public string LogLevelString => LogLevel.ToString();

        public string Message { get; set; }

        public string TimeStamp { get; set; }

        public LogSection Section { get; set; }

        public string SectionString => Section.GetName();
    }
}