using JackHenry.Blazor.Server.Hubs;
using JackHenry.Blazor.Server.Hubs.Clients;
using JackHenry.Blazor.Shared;
using JackHenry.Blazor.Workers;
using JackHenry.Data;
using Microsoft.AspNetCore.SignalR;
using RedditSharp;
using RedditSharp.Things;
using StackExchange.Redis;


public static class MiddlewareExtensions
{
    public static IServiceCollection AddDeps(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IRedditPostStreamer, RedditPostStreamer>();
        services.AddSingleton<IPostHub, PostHub>();
        //services.AddScoped<IRedditCount, RedditCount>();
        services.AddTransient<IWebAgent, BotWebAgent>(x => new BotWebAgent(
            config["Constants:UserName"],
            config["Constants:Password"],
            config["Constants:AppId"], 
         config["Constants:AppSecret"], 
          config["Constants:RedirectUri"]));
        services.AddTransient<Reddit>(x => new Reddit(x.GetService<IWebAgent>(), false));
        services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(config["Constants:RedisConnection"],
            config =>
            {
                config.AllowAdmin = true; // need this to flush database
                config.AbortOnConnectFail = false;
            }));

        return services;
    }
}

public class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();

        builder.Services.AddLogging(options => options.AddConsole());

        if (string.IsNullOrEmpty(builder.Configuration["Constants:RedisConnection"]))
        {
            throw new ArgumentException("Configuration is not being read!");
        }
        
        builder.Services.AddSignalR().AddStackExchangeRedis(builder.Configuration["Constants:RedisConnection"]);

        builder.Services.AddDeps(builder.Configuration);

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseWebAssemblyDebugging();
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseSwagger();

        app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

        app.MapRazorPages();

        app.MapDefaultControllerRoute();

        app.MapHub<PostHub>("/hubs/posts");

        app.MapFallbackToFile("index.html");

        await app.RunAsync();

    }
}
