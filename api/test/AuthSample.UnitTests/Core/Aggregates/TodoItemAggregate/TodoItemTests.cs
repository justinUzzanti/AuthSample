using AuthSample.Core.TodoItemAggregate;
using AuthSample.Core.TodoItemAggregate.Events;

namespace AuthSample.UnitTests.Core.Aggregates.TodoItemAggregate;

public class TodoItemTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_FailsWithBadTitle(string badTitle)
    {
        var act = () => new TodoItem(badTitle);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_SetsProperties()
    {
        var todo = new TodoItem("Test");

        todo.Title.Should().Be("Test");
    }

    [Fact]
    public void Done_WhenSetTrue_RegistersEvent()
    {
        var todo = new TodoItem("Test")
        {
            Done = true
        };

        todo.DomainEvents.Should().ContainSingle(x => x is TodoItemCompletedEvent);
    }
}
