using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Common.Exceptions;
using AuthSample.Core.TodoItemAggregate;
using MediatR;

namespace AuthSample.BusinessLogic.TodoItems;

public class DeleteTodoItem
{
    public class Command : IRequest
    {
        public int Id { get; set; }
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

            if (todo == null) throw new NotFoundException(nameof(TodoItem), request.Id);

            await _repository.DeleteAsync(todo, cancellationToken);
        }
    }
}
