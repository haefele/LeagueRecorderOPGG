using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Recording;
using LiteGuard;
using NeverNull;
using Newtonsoft.Json.Linq;

namespace LeagueRecorder.Windows.Recording
{
    public class RecordingService : IRecordingService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingService"/> class.
        /// </summary>
        public RecordingService()
        {
            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        public async Task<MatchInfo> GetCurrentMatchInfoFromPlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("Player", player);
            Guard.AgainstNullArgumentProperty("Player", "Username", player.Username);
            
            var content = new StringContent(string.Format("userName={0}&force=true", player.Username));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await this.CreateClient(player.Region)
                                                     .PostAsync("/summoner/ajax/spectator/", content)
                                                     .ConfigureAwait(false);
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                this.Logger.ErrorFormat("Error while getting the current match info from the Player {0}.{1}{2}", player, Environment.NewLine, responseText);
                return null;
            }

            string gameId = this.ExtractGameIdFromResponse(responseText);

            if (gameId == null)
                return null;

            return new MatchInfo
            {
                GameId = gameId,
                Region = player.Region
            };
        }
        public async Task<bool> RequestRecordingOfMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/summoner/ajax/requestRecording.json/gameId={0}", match.GameId))
                                                     .ConfigureAwait(false);
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            { 
                this.Logger.ErrorFormat("Error while requesting to record the match {0}.{1}{2}", match, Environment.NewLine, responseText);
                return false;
            }

            var responseObject = JObject.Parse(responseText);

            return responseObject.Value<bool>("success");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates an <see cref="HttpClient"/> with a base address for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        private HttpClient CreateClient(Region region)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(region.GetBaseUri());
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));

            return client;
        }
        /// <summary>
        /// Tries to extract the game-id from the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The response.</param>
        private string ExtractGameIdFromResponse(string response)
        {
            Guard.AgainstNullArgument("response", response);

            Match match = Regex.Match(response, @"/match/observer/id=(\d*)");

            if (match.Success == false)
            {
                this.Logger.ErrorFormat("Could not extract the game-id from the response.{0}{1}", Environment.NewLine, response);
                return null;
            }

            this.Logger.DebugFormat("Found match-id {0}", match.Groups[1].Value);
            return match.Groups[1].Value;
        }
        #endregion
    }
}