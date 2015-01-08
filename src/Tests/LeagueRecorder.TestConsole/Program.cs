using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Windows.Properties;
using LeagueRecorder.Windows.Storage;
using NeverNull;

namespace LeagueRecorder.TestConsole
{
    public class RootItemB : IRootItem
    {
        
    }
    public class RootItemA : IRootItem
    {
        
    }
    public interface IRootItem
    {
        
    }
    public class Root
    {
        public List<IRootItem> Items { get; set; } 
    }

    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<RootItemA>().Forward<IRootItem>().LifestyleTransient(),
                Component.For<RootItemB>().Forward<IRootItem>().LifestyleTransient(),
                Component.For<Root>().LifestyleTransient());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            var root = container.Resolve<Root>();

            //var service = new PlayerStorage(BlobCache.UserAccount, new IdentityGenerator());
            
            //var users = service.GetPlayersAsync().Result;
            //service.AddPlayerAsync(new Player {Region = Region.EuropeWest, Username = "haefele"}).Wait();
            
            //while(true)
            {
                //var spectatorService = new SpectatorService();

                //var matchInfo = new MatchInfo
                //{
                //    GameId = "1899597404",
                //    Region = Region.EuropeWest
                //};
                //spectatorService.SpectateMatchAsync(matchInfo).Wait();

                //Console.Write("Username: ");
                //var user = new Player
                //{
                //    Region = Region.EuropeWest,
                //    Username = Console.ReadLine()
                //};
                //Option<MatchInfo> currentGameResult = recordingService.GetCurrentMatchInfoFromPlayerAsync(user).Result;

                //if (currentGameResult.HasValue)
                //{
                //    Console.WriteLine("Found game " + currentGameResult.Value.GameId);
                //    Option<bool> requestRecordingResult = recordingService.RequestRecordingOfMatchAsync(currentGameResult.Value).Result;

                //    if (requestRecordingResult.HasValue && requestRecordingResult.Value)
                //        Console.WriteLine("Recording requested.");
                //}
                //else
                //{
                //    Console.WriteLine("No game found.");
                //}
            }
        }
    }
}
