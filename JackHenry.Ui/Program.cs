using JackHenry.Data;
using JackHenry.Ui.Controllers;
using JackHenry.Ui.Hubs;
using RedditSharp;
using StackExchange.Redis;
using Constants = JackHenry.Ui.Models.Constants;

namespace JackHenry.Ui;
public class Program
{
    static async Task Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        
        builder.Services.AddLogging(x =>
        {
            x.ClearProviders();
            x.AddConsole(Console.Write);
            x.SetMinimumLevel(LogLevel.Trace);
        });

        // Using Redis as SignalR backplane
        builder.Services.AddSignalR().AddStackExchangeRedis(Constants.RedisConnection);


        builder.Services.AddSingleton<IRedditPostStreamer, RedditPostStreamer>();
        builder.Services.AddScoped<IRedditController, RedditController>();
        builder.Services.AddSingleton<IPostHub, PostHub>();
        builder.Services.AddTransient<IWeatherController, WeatherForecastController>();
        builder.Services.AddTransient<IWebAgent, BotWebAgent>(x => new BotWebAgent(Constants.UserName, Constants.Password, Constants.AppId, Constants.AppSecret, Constants.RedirectUri));
        builder.Services.AddTransient<Reddit>(x => new Reddit(x.GetService<IWebAgent>(), false));
        builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(Constants.RedisConnection,
            config =>
            {
                config.AllowAdmin = true; // need this to flush database
                config.AbortOnConnectFail = false;
            }));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                    policy.SetIsOriginAllowed((host) => true);
                });
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<PostHub>("/hubs/posts");
        });

        app.MapFallbackToFile("index.html");

        await app.RunAsync();
    }
}
