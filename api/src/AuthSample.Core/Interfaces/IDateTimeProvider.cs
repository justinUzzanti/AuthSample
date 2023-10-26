namespace AuthSample.Core.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
