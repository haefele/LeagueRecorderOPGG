using System;
using Akavache;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Windows.Properties;
using LeagueRecorder.Windows.Recording;
using LeagueRecorder.Windows.Storage;
using NeverNull;

namespace LeagueRecorder.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BlobCache.ApplicationName = "LeagueRecorderOPGG";
            var service = new UserStorage(BlobCache.UserAccount, new IdentityGenerator());
            
            var users = service.GetUsersAsync().Result;
            service.AddUserAsync(new User {Region = Region.EuropeWest, Username = "haefele"}).Wait();
            
            //while(true)
            {
                var spectatorService = new SpectatorService();

                var matchInfo = new MatchInfo
                {
                    GameId = "1899597404",
                    Region = Region.EuropeWest
                };
                spectatorService.SpectateMatchAsync(matchInfo).Wait();

                //Console.Write("Username: ");
                //var user = new User
                //{
                //    Region = Region.EuropeWest,
                //    Username = Console.ReadLine()
                //};
                //Option<MatchInfo> currentGameResult = recordingService.GetCurrentMatchInfoFromUserAsync(user).Result;

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
