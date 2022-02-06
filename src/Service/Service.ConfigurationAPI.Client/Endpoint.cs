using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Client
{
    /// <summary>
    /// Endpoints available for the Configuration API
    /// </summary>
    public enum Endpoint
    {
        [Description("status/ping")]
        Ping,
        [Description("session")]
        Sessions,
        [Description("log")]
        Logs
    }

    internal static class EndpointsExtensions
    {
        /// <summary>
        /// Returns endpoint string associated with given enum
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static string GetEndpoint(this Endpoint endpoint)
        {

            var attributes = typeof(Endpoint).GetMember(endpoint.ToString())[0]
                                              .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes?[0] as DescriptionAttribute)?.Description;
        }
 
    }
}
