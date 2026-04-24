using GoodHamburguerApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguerApp.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ActionResult CustomResponse<T>(T result, string message = null) where T : class
        {
            return Ok(new ApiResponse<T>(result, message));
        }
    }
}
