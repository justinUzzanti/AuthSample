using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Common.Exceptions;
using AuthSample.Core.TodoItemAggregate;
using FluentValidation;
using MediatR;

namespace AuthSample.BusinessLogic.TodoItems;

public class UpdateTodoItem
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public bool Done { get; set; }
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

    public class Handler : IRequestHandler<Command>
    {
        private readonly IRepository<TodoItem> _repository;

        public Handler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (todo == null)
                throw new NotFoundException(nameof(TodoItem), request.Id);

            todo.Title = request.Title;
            todo.Done = request.Done;

            await _repository.UpdateAsync(todo, cancellationToken);
        }
    }
}
