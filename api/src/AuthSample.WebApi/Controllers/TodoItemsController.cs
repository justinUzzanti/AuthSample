using AuthSample.BusinessLogic.TodoItems;
using Microsoft.AspNetCore.Mvc;

namespace AuthSample.WebApi.Controllers;

public class TodoItemsController : ApiControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<GetTodoItemSummaries.Result>> Get() => Mediator.Send(new GetTodoItemSummaries.Query());

    [HttpGet("{id}")]
    public Task<GetTodoItemDetail.Result> GetSingle(int id) => Mediator.Send(new GetTodoItemDetail.Query { Id = id });

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, UpdateTodoItem.Command command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateTodoItem.Command command)
    {
        var newId = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetSingle), new { id = newId }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoItem.Command() { Id = id });

        return NoContent();
    }
}