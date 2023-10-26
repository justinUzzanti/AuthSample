using Centeva.DomainModeling.EFCore;
using Centeva.DomainModeling.Interfaces;

namespace AuthSample.Infrastructure.Persistence;

public class EfRepository<TEntity> : BaseRepository<TEntity> where TEntity : class, IAggregateRoot
{
    public EfRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}