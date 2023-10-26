namespace AuthSample.Core.Interfaces;

public interface ITimestamped
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastUpdatedAt { get; set; }
}