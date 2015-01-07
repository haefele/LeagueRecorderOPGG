using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LiteGuard;

namespace LeagueRecorder.Windows.League
{
    public class PlayerService : IPlayerService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        public PlayerService()
        {
            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks whether a player with the specified <paramref name="username" /> in the specified <paramref name="region" /> exists.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="region">The region.</param>
        public async Task<bool> PlayerExists(string username, Region region)
        {
            Guard.AgainstNullArgument("username", username);

            this.Logger.DebugFormat("Checking if a player with username {0} exists in the region {1}.", username, region.GetReadableString());

            HttpResponseMessage response = await this.CreateClient(region)
                .GetAsync(string.Format("/summoner/userName={0}", username))
                .ConfigureAwait(false);
            
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            this.Logger.DebugFormat("Got response from the web-service: {0}", responseText);

            if (response.IsSuccessStatusCode == false)
            {
                this.Logger.ErrorFormat("Error while trying to validate a player username. {0}", responseText);
                return false;
            }

            string playerId = this.ExtractPlayerIdFromResponse(responseText);
            return playerId != null;
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
        /// Tries to extract the player-id from the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The response.</param>
        private string ExtractPlayerIdFromResponse(string response)
        {
            Guard.AgainstNullArgument("response", response);

            this.Logger.DebugFormat("Trying to extract the player-id from the response: {0}", response);

            Match match = Regex.Match(response, @"SummonerRefresh.RefreshUser\(this, (\d*)\)");

            if (match.Success == false)
            {
                this.Logger.ErrorFormat("Could not extract the player-id from the response: {0}", response);
                return null;
            }

            this.Logger.DebugFormat("Found player-id {0}", match.Groups[1].Value);
            return match.Groups[1].Value;
        }
        #endregion
    }
}