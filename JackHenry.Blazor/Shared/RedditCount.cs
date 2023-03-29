using StackExchange.Redis;

namespace JackHenry.Blazor.Workers
{
    //public class RedditCount : IRedditCount
    //{
    //    private readonly IConnectionMultiplexer redis;
    //    private readonly CancellationTokenSource cancellationToken;
    //    private readonly IDatabase db;
    //    public int PostCount { get; set; }

    //    public RedditCount(IConnectionMultiplexer redis)
    //    {
    //        this.redis = redis;
    //        this.cancellationToken = new CancellationTokenSource();
    //        this.db = this.redis.GetDatabase();
    //    }

    //    public async Task UpdateCount()
    //    {
    //        while (!this.cancellationToken.IsCancellationRequested)
    //        {
    //            var stream = await this.db.StreamReadAsync(new[]
    //            {
    //                new StreamPosition("posts", "0-0")
    //            });

    //            if (stream.Length == 0)
    //            {
    //                continue;
    //            }

    //            //this.postCount = stream.First().Entries.Length;
    //            this.PostCount = stream.First().Entries.Count();
    //        }
    //    }
    //}
}
