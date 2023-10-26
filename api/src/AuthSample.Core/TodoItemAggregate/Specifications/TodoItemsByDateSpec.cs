using Ardalis.Specification;

namespace AuthSample.Core.TodoItemAggregate.Specifications;

public class TodoItemsByDateSpec : Specification<TodoItem>
{
    public TodoItemsByDateSpec()
    {
        Query
            .OrderBy(x => x.CreatedAt);
    }
}