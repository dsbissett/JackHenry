using JackHenry.Ui.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace JackHenry.Ui.Hubs
{
    public class PostHub : Hub<IPostClient>, IPostHub
    {
        private readonly IHubContext<PostHub, IPostClient> hubContext;
        private readonly IConnectionMultiplexer redis;
        private readonly CancellationTokenSource cancellationToken;
        private readonly IDatabase db;

        public PostHub(IHubContext<PostHub, IPostClient> hubContext, IConnectionMultiplexer redis)
        {
            this.hubContext = hubContext;
            this.redis = redis;
            this.db = this.redis.GetDatabase();
            this.cancellationToken = new CancellationTokenSource();
        }

        public override async Task OnConnectedAsync()
        {
            await Task.WhenAll(this.UpdatePostCount(), this.UpdatePostTitles(), this.UpdatePostAuthors());
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            this.cancellationToken.Cancel();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task UpdatePostCount()
        {
            while (!this.cancellationToken.IsCancellationRequested)
            {
                var stream = await this.db.StreamReadAsync(new[]
                {
                    new StreamPosition(key: "posts", position: "0-0")
                });

                if (stream.Length == 0)
                {
                    continue;
                }

                var count = stream.First().Entries.Length;

                await this.hubContext.Clients.All.SendPostCount(count);
                await Task.Delay(1000);
            }
        }

        public async Task UpdatePostTitles()
        {
            while (!this.cancellationToken.IsCancellationRequested)
            {
                var stream = await this.db.StreamReadAsync(new[]
                {
                    new StreamPosition(key: "posts", position: "0-0")
                });
                if (stream.Length == 0)
                {
                    continue;
                }

                var titles = stream.First().Entries
                    .SelectMany(x => x.Values)
                    .Where(x => x.Name == "title")
                    .Select(x => x.Value.ToString()).ToList();

                await this.hubContext.Clients.All.SendPostTitles(titles);
                await Task.Delay(1000);
            }
        }

        public async Task UpdatePostAuthors()
        {
            while (!this.cancellationToken.IsCancellationRequested)
            {
                var stream = await this.db.StreamReadAsync(new[]
                {
                    new StreamPosition(key: "posts", position: "0-0")
                });

                if (stream.Length == 0)
                {
                    continue;
                }

                var authors = stream.First().Entries
                    .SelectMany(x => x.Values)
                    .Where(x => x.Name == "authorname")
                    .Select(x => x.Value.ToString()).ToList();

                await this.hubContext.Clients.All.SendPostAuthors(authors);
                await Task.Delay(1000);
            }
        }
    }
}
