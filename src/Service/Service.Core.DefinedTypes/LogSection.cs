using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.DefinedTypes
{
    public enum LogSection
    {
        [Description("Main server")]
        MAIN_SERVER,
        [Description("Communication resolver")]
        COMMUNICATION_RESOLVER,
        [Description("Communication executor")]
        COMMUNICATION_EXECUTOR,
        [Description("Communication dispatcher")]
        COMMUNICATION_DISPATCHER,
        [Description("Storage")]
        STORAGE,
        [Description("Token")]
        TOKEN
    }

    public static class LogSectionExtensions
    {
        /// <summary>
        /// Returns endpoint string associated with given enum
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static string GetName(this LogSection endpoint)
        {

            var attributes = typeof(LogSection).GetMember(endpoint.ToString())[0]
                                              .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes?[0] as DescriptionAttribute)?.Description;
        }

    }
}
