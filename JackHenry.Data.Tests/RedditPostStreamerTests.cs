using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using RedditSharp;
using RedditSharp.Things;

namespace JackHenry.Data.Tests
{
    public class RedditPostStreamerTests
    {
        // DoWork() is the only method worth testing in this class
        // It runs inside of a BackgroundWorker where it retrieves 
        // data from RedditSharp's AsyncEnumerable and stuffs it into
        // a ConcurrentBag.  It increments a PostCount field using
        // Interlocked.Increment because it's the cheapest operation
        // for a total in this way.
        [Fact]
        public void RunAsyncShouldRun()
        {
            var fixture = new Fixture();

            var mockLogger = new Mock<ILogger<RedditPostStreamer>>();

            var mock = new Mock<Reddit>();

            var mockSubreddit = new Mock<Subreddit>();

            //var sut = new RedditPostStreamer(mockLogger.Object, mock.Object);

            //sut.Run();
        }


    }
}