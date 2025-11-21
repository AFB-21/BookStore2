using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Features.Books.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            var result = await _mediator.Send(new CreateBookCommand(dto));
            return CreatedAtAction(nameof(Get), new {id=result.Id},result);
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

        //[HttpGet]
        //public async Task<IActionResult> GetBooksPaginated(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 10,
        //    [FromQuery] string? filter = null,
        //    [FromQuery] string? sortBy = null,
        //    [FromQuery] bool desc = false
        //    )
        //{
        //    var query = new GetAllBookspaginatedQuery(page, pageSize, filter, sortBy, desc);
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDTO request)
        {
            if (id != request.Id)
                return BadRequest();

            await _mediator.Send(new UpdateBookCommand(id,request));

            return Ok(new { message = "BookUpdated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));

            return Ok(new { message = "BookDeleted" });
        }
    }
}
