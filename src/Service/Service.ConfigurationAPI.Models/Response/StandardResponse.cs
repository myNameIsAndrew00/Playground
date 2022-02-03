using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Models.Response
{
    /// <summary>
    /// Represents the standard response returned by configuration API
    /// </summary>
    public class StandardResponse<DTOModel>
    {
        public string? Message { get; init; }

        public DTOModel? Data { get; init; }

        public ResponseCode Code { get; init; }
    }

    /// <summary>
    /// Represents the standard response returned by configuration API
    /// </summary>
    public class StandardResponse : StandardResponse<object>
    {
    }
}
