using BookStore.Application.DTOs.Author;
using BookStore.Application.Features.Authors.Commands.Models;
using BookStore.Application.Features.Authors.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorsController(IMediator mediator)  
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("create")]
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDTO dto)
        {
            var result = await _mediator.Send(new CreateAuthorCommand(dto));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetAuthorQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAuthorDTO request)
        {
            var result = await _mediator.Send(new UpdateAuthorCommand(id, request));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteAuthorCommand(id));

            return Ok(new { message = "AuthorDeleted" });
        }
    }
}
