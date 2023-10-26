using System.Reflection;
using Centeva.DomainModeling.EFCore;
using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Interfaces;
using AuthSample.Core.TodoItemAggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthSample.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher domainEventDispatcher, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();

        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await _domainEventDispatcher.DispatchAndClearEvents(this.GetEntitiesWithEvents(), cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    private void UpdateTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<ITimestamped>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTimeProvider.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastUpdatedAt = _dateTimeProvider.UtcNow;
                    break;
            }
        }
    }
}