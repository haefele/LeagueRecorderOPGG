using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LiteGuard;
using Newtonsoft.Json.Linq;

namespace LeagueRecorder.Windows.League
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
        /// <summary>
        /// Asynchronously returns the current <see cref="MatchInfo" /> of the specified <paramref name="player" />.
        /// </summary>
        /// <param name="player">The player.</param>
        public async Task<MatchInfo> GetCurrentMatchInfoFromPlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("Player", player);
            Guard.AgainstNullArgumentProperty("Player", "Username", player.Username);
            
            this.Logger.DebugFormat("Getting the current match from the player '{0}'.", player);

            var content = new StringContent(string.Format("userName={0}&force=true", player.Username));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await this.CreateClient(player.Region)
                                                     .PostAsync("/summoner/ajax/spectator/", content)
                                                     .ConfigureAwait(false);
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            this.Logger.DebugFormat("Got response from the web-service: {0}", responseText);

            if (response.IsSuccessStatusCode == false)
            {
                this.Logger.ErrorFormat("Error while getting the current match info from the Player {0}. {1}", player, responseText);
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
        /// <summary>
        /// Asynchronously requests to record the specified <paramref name="match" />.
        /// Returns whether the match is beeing recorded.
        /// </summary>
        /// <param name="match">The match.</param>
        public async Task<bool> RequestRecordingOfMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            this.Logger.DebugFormat("Requesting that match '{0}' is beeing recorded.", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/summoner/ajax/requestRecording.json/gameId={0}", match.GameId))
                                                     .ConfigureAwait(false);
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            this.Logger.DebugFormat("Got response from the web-service: {0}", responseText);

            if (response.IsSuccessStatusCode == false)
            { 
                this.Logger.ErrorFormat("Error while requesting to record the match {0}. {1}", match, responseText);
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

            this.Logger.DebugFormat("Trying to extract the current game-id from the response: {0}", response);

            Match match = Regex.Match(response, @"/match/observer/id=(\d*)");

            if (match.Success == false)
            {
                this.Logger.ErrorFormat("Could not extract the game-id from the response: {0}", response);
                return null;
            }

            this.Logger.DebugFormat("Found match-id {0}", match.Groups[1].Value);
            return match.Groups[1].Value;
        }
        #endregion
    }
}