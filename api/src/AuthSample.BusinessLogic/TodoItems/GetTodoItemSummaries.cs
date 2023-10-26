using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.TodoItemAggregate.Specifications;
using AuthSample.Core.TodoItemAggregate;
using MediatR;

namespace AuthSample.BusinessLogic.TodoItems;

public class GetTodoItemSummaries
{
    public class Query : IRequest<IReadOnlyList<Result>>
    {
    }

    public class Result
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public DateTime? Reminder { get; set; }
        public PriorityLevel Priority { get; set; }
    }

    public class Handler : IRequestHandler<Query, IReadOnlyList<Result>>
    {
        private readonly IReadRepository<TodoItem> _repository;

        public Handler(IReadRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Result>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var summaries = await _repository.ListAsync(new TodoItemsByDateSpec(), todo => new Result
            {
                Id = todo.Id,
                Title = todo.Title,
                Priority = todo.Priority,
                Reminder = todo.Reminder
            }, cancellationToken);

            return summaries;
        }
    }
}
