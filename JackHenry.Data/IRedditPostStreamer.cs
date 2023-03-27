using RedditSharp.Things;

namespace JackHenry.Data;

public interface IRedditPostStreamer
{
    Task StreamToRedisAsync(Post post);

    void Run();

    void Stop();

    Task FlushDatabaseAsync();
}