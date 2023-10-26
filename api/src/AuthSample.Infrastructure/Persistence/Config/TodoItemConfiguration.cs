using AuthSample.Core.TodoItemAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSample.Infrastructure.Persistence.Config;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();
    }
}