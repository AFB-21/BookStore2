using BookStore.Application.DTOs.Category;
using BookStore.Application.Features.Categories.Commands.Models;
using BookStore.Application.Features.Categories.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id)
        {

            var result = await _mediator.Send(new GetCategoryQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }


        //[HttpGet]
        //[Route("paginated")]
        //public async Task<IActionResult> GetAllsPaginated(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 10
        //    )
        //{
        //    var query = new GetAllCategoriesPaginatedQuery(page, pageSize);
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDTO request)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(id, request));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));

            return Ok(new { message = "Category Deleted" });
        }
    }
}
