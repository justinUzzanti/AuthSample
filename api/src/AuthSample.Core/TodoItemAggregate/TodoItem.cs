using Ardalis.GuardClauses;
using Centeva.DomainModeling;
using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Interfaces;
using AuthSample.Core.TodoItemAggregate.Events;

namespace AuthSample.Core.TodoItemAggregate;

public class TodoItem : BaseEntity<int>, IAggregateRoot, ITimestamped
{
    public string Title { get; set; }
    public string? Note { get; set; }
    public DateTime? Reminder { get; set; }
    public PriorityLevel Priority { get; set; }

    public TodoItem(string title)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
    }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (!_done)
            {
                RegisterDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastUpdatedAt { get; set; }
}