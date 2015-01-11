using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LeagueRecorder.Windows.Extensions;
using LiteGuard;

namespace LeagueRecorder.Windows.League
{
    public class SpectatorService : ISpectatorService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SpectatorService"/> class.
        /// </summary>
        public SpectatorService()
        {
            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Downloads the spectate-file and starts the League of Legends client to spectate the specified <paramref name="match" />.
        /// </summary>
        /// <param name="match">The match.</param>
        public async Task<bool> SpectateMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            this.Logger.DebugFormat("Trying to spectate the match '{0}'.", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/match/observer/id={0}", match.GameId))
                                                     .ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                this.Logger.DebugFormat("No successfull response from the web-service. Can't spectate the game.");
                return false;
            }

            string commands = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this.Logger.DebugFormat("Got the commands to spectate the match.");

            string filePath = await CreateBatchFile(commands).ConfigureAwait(false);

            this.Logger.DebugFormat("Starting the bat-file.");
            await Process.Start(filePath).WaitForExitAsync().ConfigureAwait(false);

            this.Logger.DebugFormat("Deleting the temporary file.");
            File.Delete(filePath);

            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="region">The region.</param>
        private HttpClient CreateClient(Region region)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(region.GetBaseUri())
            };
        }
        /// <summary>
        /// Creates the batch file containing the specified <paramref name="commands"/>.
        /// </summary>
        /// <param name="commands">The commands.</param>
        private async Task<string> CreateBatchFile(string commands)
        {
            string fileName = string.Format("{0}.bat", Guid.NewGuid().ToString("N"));
            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            this.Logger.DebugFormat("Creating a temporary bat-file at '{0}'.", filePath);

            using (var fileStream = File.Open(filePath, FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                await writer.WriteAsync(commands).ConfigureAwait(false);
            }

            return filePath;
        }
        #endregion
    }
}