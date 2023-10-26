using Centeva.DomainModeling;

namespace AuthSample.Core.TodoItemAggregate.Events;

public class TodoItemCompletedEvent : BaseDomainEvent
{
    public TodoItem Item { get; private set; }

    public TodoItemCompletedEvent(TodoItem todoItem)
    {
        Item = todoItem;
    }
}