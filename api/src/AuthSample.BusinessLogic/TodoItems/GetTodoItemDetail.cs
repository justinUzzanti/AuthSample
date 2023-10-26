using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Common.Exceptions;
using AuthSample.Core.TodoItemAggregate;
using MediatR;

namespace AuthSample.BusinessLogic.TodoItems;

public class GetTodoItemDetail
{
    public class Query : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime? Reminder { get; set; }
        public bool Done { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IReadRepository<TodoItem> _repository;

        public Handler(IReadRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var todo = await _repository.GetByIdAsync(request.Id, cancellationToken)
                       ?? throw new NotFoundException(nameof(TodoItem), request.Id);

            return new Result
            {
                Id = todo.Id,
                Title = todo.Title,
                Note = todo.Note,
                Reminder = todo.Reminder,
                Done = todo.Done
            };
        }
    }
}
