using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Recording;
using LeagueRecorder.Windows.Extensions;

namespace LeagueRecorder.Windows.Recording
{
    public class SpectatorService : ISpectatorService
    {
        public async Task ExecuteSpectatorCommandsAsync(string commands)
        {
            string filePath = await CreateBatchFile(commands).ConfigureAwait(false);

            await Process.Start(filePath).WaitForExitAsync().ConfigureAwait(false);
            File.Delete(filePath);
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