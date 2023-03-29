using StackExchange.Redis;

namespace JackHenry.Blazor.Workers;

public interface IRedditCount
{
    Task UpdateCount();

    int PostCount { get; set; }
}