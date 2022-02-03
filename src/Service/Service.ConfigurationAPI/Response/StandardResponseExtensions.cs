using Microsoft.AspNetCore.Mvc;
using Service.ConfigurationAPI.Models.Response;

namespace Service.ConfigurationAPI.Response
{
    public static class StandardResponseExtensions
    {
        public static IActionResult StandardResponse(this ControllerBase controller, object data) => new JsonResult(new StandardResponse() { Data = data });

        public static IActionResult StandardResponse<DTOModel>(this ControllerBase controller, DTOModel data) => new JsonResult(new StandardResponse<DTOModel>() { Data = data });
    }
}
