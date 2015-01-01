using System;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Windows.Recording;
using NeverNull;

namespace LeagueRecorder.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //while(true)
            {
                var recordingService = new RecordingService();
                var spectatorService = new SpectatorService();

                var matchInfo = new MatchInfo("1899597404", Region.EuropeWest);
                var commands = recordingService.GetCommandsToStartSpectatingAsync(matchInfo).Result;

                spectatorService.ExecuteSpectatorCommandsAsync(commands.Value).Wait();
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
