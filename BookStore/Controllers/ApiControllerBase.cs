using BookStore.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    "Error.Validation" => BadRequest(new { error = result.Error.Message }),
                    "Error.Unauthorized" => Unauthorized(new { error = result.Error.Message }),
                    "Error.Forbidden" => Forbid(),
                    "Error.Conflict" => Conflict(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }

            return Ok(result.Value);
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return Ok();

            return result.Error.Code switch
            {
                "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                "Error.Validation" => BadRequest(new { error = result.Error.Message }),
                "Error.Unauthorized" => Unauthorized(new { error = result.Error.Message }),
                "Error.Forbidden" => Forbid(),
                "Error.Conflict" => Conflict(new { error = result.Error.Message }),
                _ => BadRequest(new { error = result.Error.Message })
            };
        }
    }
}
