using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Features.Books.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            var result = await _mediator.Send(new CreateBookCommand(dto));
            if(result.IsFailure){
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    "Error.Validation" => BadRequest(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }
            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetBookQuery(id));
            if (result == null)
                return NotFound(new { error = $"Book with id '{id}' was not found." });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BookDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());
            return Ok(result);
        }


        [HttpGet]
        [Route("paginated")]
        [ProducesResponseType(typeof(List<BookDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllsPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var query = new GetAllBooksPaginatedQuery(page, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BookDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDTO request)
        {
            var result = await _mediator.Send(new UpdateBookCommand(id, request));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    "Error.Validation" => BadRequest(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }
            return Ok(new { message = "Book deleted successfully", book = result.Value });
        }
    }
}
