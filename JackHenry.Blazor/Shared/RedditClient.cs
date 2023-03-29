using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.FileIO;

namespace JackHenry.Blazor.Shared
{
    public class RedditHttpClient
    {
        private readonly HttpClient http;

        public RedditHttpClient(HttpClient http)
        {
            this.http = http;
        }

        public async Task StartPostCount()
        {
            await this.http.GetAsync("api/Reddit");
        }

    }
    //public class RedditClient : IAsyncDisposable
    //{
    //    public const string HUB_URL = "/hubs/posts";
        
    //    private string hubUrl;

    //    private bool isStarted;

    //    private HubConnection hubConnection;

    //    public RedditClient(string siteUrl)
    //    {
    //        this.hubUrl = string.Concat(siteUrl.TrimEnd('/'), HUB_URL);
    //    }

    //    public async Task StartAsync()
    //    {
    //        if (!this.isStarted)
    //        {
    //            this.hubConnection = new HubConnectionBuilder()
    //                .WithUrl(this.hubUrl)
    //                .Build();

    //            await this.hubConnection.StartAsync();

    //            this.isStarted = true;
    //        }
    //    }

    //    public async Task SendPostCountAsync(int postCount)
    //    {
    //        if (!this.isStarted)
    //        {
    //            throw new InvalidOperationException("Cannot do the needful into the hub!");
    //        }

    //        await this.hubConnection.SendAsync(Messages.POSTCOUNT, postCount);
    //    }

    //    public async Task StopAsync()
    //    {
    //        await this.hubConnection.StopAsync();

    //        await this.hubConnection.DisposeAsync();

    //        this.hubConnection = null;

    //        this.isStarted = false;
    //    }

    //    public ValueTask DisposeAsync()
    //    {
    //        // await this.StopAsync();
    //        throw new NotImplementedException();
    //    }
    //}
}
