using AuthSample.Core.TodoItemAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSample.Core.TodoItemAggregate.Handlers;

public class TodoItemCompletedNotifier : INotificationHandler<TodoItemCompletedEvent>
{
    private readonly ILogger<TodoItemCompletedNotifier> _logger;

    public TodoItemCompletedNotifier(ILogger<TodoItemCompletedNotifier> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        // You'd probably be sending an email or a push notification here
        _logger.LogInformation("Todo item {@Item} completed", notification.Item);

        return Task.CompletedTask;
    }
}
