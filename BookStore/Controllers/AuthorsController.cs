using BookStore.Application.DTOs.Author;
using BookStore.Application.Features.Authors.Commands.Models;
using BookStore.Application.Features.Authors.Queries.Models;
using BookStore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthorStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Roles = "Admin,Author")]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDTO dto)
        {
            var result = await _mediator.Send(new CreateAuthorCommand(dto));
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

            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetAuthorQuery(id));
            if (result == null)
                return NotFound(new { error = "Author with id " + id + " was not found" });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AuthorSummaryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAuthorsQuery());
            return Ok(result);
        }


        //[HttpGet]
        //[Route("paginated")]
        //public async Task<IActionResult> GetAllsPaginated(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 10
        //    )
        //{
        //    var query = new GetAllAuthorsPaginatedQuery(page, pageSize);
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAuthorDTO request)
        {
            var result = await _mediator.Send(new UpdateAuthorCommand(id, request));
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

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    "Error.Unauthorized" => Unauthorized(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }

            return Ok(new { message = "AuthorDeleted" });
        }
    }
}
