using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface IRecordingService
    {
        Task<MatchInfo> GetCurrentMatchInfoFromPlayerAsync(Player player);

        Task<bool> RequestRecordingOfMatchAsync(MatchInfo match);
    }
}