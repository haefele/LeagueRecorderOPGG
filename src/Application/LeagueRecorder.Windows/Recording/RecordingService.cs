using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Recording;
using LiteGuard;
using NeverNull;
using Newtonsoft.Json.Linq;

namespace LeagueRecorder.Windows.Recording
{
    public class RecordingService : IRecordingService
    {
        #region Methods
        public async Task<Option<MatchInfo>> GetCurrentGameIdFromUserAsync(User user)
        {
            Guard.AgainstNullArgument("user", user);
            Guard.AgainstNullArgumentProperty("user", "Username", user.Username);
            
            var content = new StringContent(string.Format("userName={0}&force=true", user.Username));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await this.CreateClient(user.Region)
                                                     .PostAsync("/summoner/ajax/spectator/", content)
                                                     .ConfigureAwait(false);

            Option<string> gameId = await this.ExtractGameIdFromResponse(response).ConfigureAwait(false);

            if (gameId.IsEmpty)
                return Option.None;

            return Option.From(new MatchInfo(gameId.Value, user.Region));
        }
        public async Task<Option<bool>> RequestRecordingOfGameAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/summoner/ajax/requestRecording.json/gameId={0}", match.GameId))
                                                     .ConfigureAwait(false);
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JObject.Parse(responseText);

            return Option.From(responseObject.Value<bool>("success"));
        }

        public async Task<Option<string>> GetCommandsToStartSpectatingAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            HttpResponseMessage response = await this.CreateClient(match.Region)
                                                     .GetAsync(string.Format("/match/observer/id={0}", match.GameId))
                                                     .ConfigureAwait(false);

            return Option.None;
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
        private async Task<Option<string>> ExtractGameIdFromResponse(HttpResponseMessage response)
        {
            Guard.AgainstNullArgument("response", response);

            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Match match = Regex.Match(responseText, @"/match/observer/id=(\d*)");

            if (match.Success == false)
                return Option.None;

            return Option.From(match.Groups[1].Value);
        }
        #endregion
    }
}