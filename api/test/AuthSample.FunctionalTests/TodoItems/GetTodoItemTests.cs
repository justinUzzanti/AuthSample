using Ardalis.HttpClientTestExtensions;
using AuthSample.BusinessLogic.TodoItems;

namespace AuthSample.FunctionalTests.TodoItems;

public class GetTodoItemTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetTodoItemTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenNotFound_Produces404()
    {
        var badTodoItemId = -1;

        await _client.GetAndEnsureNotFoundAsync(BuildUrl(badTodoItemId));
    }

    [Fact]
    public async Task WhenFound_ReturnsDetails()
    {
        var result = await _client.GetAndDeserializeAsync<GetTodoItemDetail.Result>(BuildUrl(SeedData.TodoItem1.Id));

        result.Should().NotBeNull();
        result.Id.Should().Be(SeedData.TodoItem1.Id);
        result.Title.Should().Be(SeedData.TodoItem1.Title);
        result.Note.Should().Be(SeedData.TodoItem1.Note);
        result.Reminder.Should().Be(SeedData.TodoItem1.Reminder);
        result.Done.Should().Be(SeedData.TodoItem1.Done);
    }

    private string BuildUrl(int itemId) => $"api/TodoItems/{itemId}";
}
