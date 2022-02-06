using Microsoft.AspNetCore.Mvc;
using Service.ConfigurationAPI.Models.Response;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.ConfigurationAPI.Response
{
    public static class StandardResponseExtensions
    {
        public static IActionResult StandardResponse(this ControllerBase controller, object data) => new JsonResult(
            new StandardResponse() { Data = data },
            new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } 
         });

        public static IActionResult StandardResponse<DTOModel>(this ControllerBase controller, DTOModel data) => new JsonResult(
            new StandardResponse<DTOModel>() { Data = data }, 
            new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } 
         });
    }
}
