using BookStore.Application.DTOs.Category;
using BookStore.Application.Features.Categories.Commands.Models;
using BookStore.Application.Features.Categories.Queries.Models;
using BookStore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result);
        }

        //[Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetCategoryQuery(id));
            if (result == null) return NotFound(new { error = $"Category with id '{id}' was not found." });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDTO request)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(id, request));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
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
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "Error.NotFound" => NotFound(new { error = result.Error.Message }),
                    _ => BadRequest(new { error = result.Error.Message })
                };
            }

            return Ok(new { message = "Category Deleted" });
        }
    }
}
