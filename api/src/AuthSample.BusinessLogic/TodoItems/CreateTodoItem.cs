using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.TodoItemAggregate;
using FluentValidation;
using MediatR;

namespace AuthSample.BusinessLogic.TodoItems;

public class CreateTodoItem
{
    public class Command : IRequest<int>
    {
        public string Title { get; set; } = String.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IRepository<TodoItem> _repository;

        public Handler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = new TodoItem(request.Title);

            await _repository.AddAsync(todo, cancellationToken);

            return todo.Id;
        }
    }
}
