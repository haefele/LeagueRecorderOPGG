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
            while(true)
            {
                var recordingService = new RecordingService();
                var spectatorService = new SpectatorService();
                
                var matchInfo = new MatchInfo("1895547193", Region.EuropeWest);
                var commands = recordingService.GetCommandsToStartSpectatingAsync(matchInfo).Result;

                spectatorService.ExecuteSpectatorCommandsAsync(commands.Value).Wait();

                var user = new User
                {
                    Region = Region.EuropeWest,
                    Username = "haefele"
                };
                Option<MatchInfo> currentGameResult = recordingService.GetCurrentGameIdFromUserAsync(user).Result;

                if (currentGameResult.HasValue)
                {
                    Console.WriteLine("Found game " + currentGameResult.Value.GameId);
                    Option<bool> requestRecordingResult = recordingService.RequestRecordingOfGameAsync(currentGameResult.Value).Result;
                }

                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }
    }
}
