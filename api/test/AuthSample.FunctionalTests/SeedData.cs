using AuthSample.Core.TodoItemAggregate;
using AuthSample.Infrastructure.Persistence;

namespace AuthSample.FunctionalTests;

public static class SeedData
{
    public static readonly TodoItem TodoItem1 = new("Test Todo 1");

    public static void PopulateTestData(ApplicationDbContext dbContext)
    {
        // Remove existing entities
        foreach (var item in dbContext.TodoItems)
        {
            dbContext.Remove(item);
        }

        dbContext.SaveChanges();

        dbContext.TodoItems.Add(TodoItem1);

        dbContext.SaveChanges();
    }
}