using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Recording;
using LeagueRecorder.Windows.Extensions;
using LiteGuard;
using NeverNull;

namespace LeagueRecorder.Windows.Recording
{
    public class SpectatorService : ISpectatorService
    {
        public async Task<bool> SpectateMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/match/observer/id={0}", match.GameId))
                                                     .ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
                return false;

            string commands = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            string filePath = await CreateBatchFile(commands).ConfigureAwait(false);

            await Process.Start(filePath).WaitForExitAsync().ConfigureAwait(false);
            File.Delete(filePath);

            return true;
        }

        private HttpClient CreateClient(Region region)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(region.GetBaseUri())
            };
        }
        private static async Task<string> CreateBatchFile(string commands)
        {
            string fileName = string.Format("{0}.bat", Guid.NewGuid().ToString("N"));
            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            using (var fileStream = File.Open(filePath, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                await writer.WriteAsync(commands).ConfigureAwait(false);
            }

            return filePath;
        }
    }
}