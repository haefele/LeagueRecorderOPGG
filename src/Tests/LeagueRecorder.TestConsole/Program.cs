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
                RecordingService service = new RecordingService();

                var user = new User
                {
                    Region = Region.EuropeWest,
                    Username = "haefele"
                };

                var matchInfo = new MatchInfo("1895547193", Region.EuropeWest);

                var a = service.GetCommandsToStartSpectatingAsync(matchInfo).Result;

                Option<MatchInfo> currentGameResult = service.GetCurrentGameIdFromUserAsync(user).Result;

                if (currentGameResult.HasValue)
                {
                    Console.WriteLine("Found game " + currentGameResult.Value.GameId);
                    Option<bool> requestRecordingResult = service.RequestRecordingOfGameAsync(currentGameResult.Value).Result;
                }

                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }
    }
}
