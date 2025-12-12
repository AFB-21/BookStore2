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
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            var result = await _mediator.Send(new CreateBookCommand(dto));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetBookQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());
            return Ok(result);
        }


        [HttpGet]
        [Route("paginated")]
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDTO request)
        {
            var result = await _mediator.Send(new UpdateBookCommand(id, request));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));

            return Ok(new { message = "BookDeleted" });
        }
    }
}
