using System.ComponentModel;
using Microsoft.Extensions.Logging;
using RedditSharp;
using RedditSharp.Things;
using StackExchange.Redis;

namespace JackHenry.Data
{
    public class RedditPostStreamer : IRedditPostStreamer
    {
        private readonly ILogger<RedditPostStreamer> logger;

        private readonly IConnectionMultiplexer redis;
        
        private readonly Reddit reddit;

        private readonly BackgroundWorker backgroundWorker;

        private CancellationTokenSource cancellationTokenSource;

        private bool reset = false;

        private int postCount;

        public RedditPostStreamer(ILogger<RedditPostStreamer> logger, IConnectionMultiplexer redis, Reddit reddit)
        {
            this.logger = logger;

            this.redis = redis;

            this.reddit = reddit;

            this.backgroundWorker = new BackgroundWorker();

            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public async void Init()
        {
            var db = this.redis.GetDatabase();

            if (!(await db.KeyExistsAsync("posts")) ||
                (await db.StreamGroupInfoAsync("posts")).All(x => x.Name != "posts_group"))
            {
                await db.StreamCreateConsumerGroupAsync("posts", "posts_group", "0-0", true);
            }
        }

        public async Task StreamToRedisAsync(Post post)
        {
            this.logger.LogInformation("Sending a post to Redis...");
            
            //var redis = await ConnectionMultiplexer.ConnectAsync("127.0.0.1:6379");
            var db = this.redis.GetDatabase();

            var values = new NameValueEntry[]
            {
            new (nameof(post.Title).ToLower(), post.Title),
            new (nameof(post.SubredditName).ToLower(), post.SubredditName),
            new (nameof(post.Upvotes).ToLower(), post.Upvotes),
            new (nameof(post.Downvotes).ToLower(), post.Downvotes),
            new (nameof(post.AuthorName).ToLower(), post.AuthorName),
            new (nameof(post.Url).ToLower(), post.Url.ToString())
            };
            
            await db.StreamAddAsync("posts", values);

            Interlocked.Increment(ref this.postCount);

            this.logger.LogInformation($"[{this.postCount}] posts added to Redis!");
        }

        public void Stop()
        {
            this.logger.LogInformation("Stop requested!");
            
            this.backgroundWorker.CancelAsync();

            this.cancellationTokenSource.Cancel();

            this.reset = true;
        }

        public void Run()
        {
            if (this.reset && this.cancellationTokenSource.IsCancellationRequested)
            {
                this.reset = false;
                this.cancellationTokenSource = new CancellationTokenSource();
            }
            this.backgroundWorker.DoWork += this.DoWork!;

            this.backgroundWorker.WorkerSupportsCancellation = true;

            this.backgroundWorker.RunWorkerAsync();

            this.logger.LogInformation("Background Worker Started!");
        }

        public async Task FlushDatabaseAsync()
        {
            await this.redis.GetServer("127.0.0.1", 6379).FlushDatabaseAsync();
        }

        private async void DoWork(object sender, DoWorkEventArgs e)
        {
            var postStream = this.reddit.RSlashAll.GetPosts(Subreddit.Sort.New).Stream();

            postStream.Subscribe(onNext: async (post) =>
            {
                if (e.Cancel)
                {
                    return;
                }

                await this.StreamToRedisAsync(post);

            }, token: this.cancellationTokenSource.Token);

            await postStream.Enumerate(this.cancellationTokenSource.Token);
        }
    }
}