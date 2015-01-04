using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using NeverNull;

namespace LeagueRecorder.Abstractions.Recording
{
    public interface IRecordingService : IService
    {
        Task<MatchInfo> GetCurrentMatchInfoFromPlayerAsync(Player player);

        Task<bool> RequestRecordingOfMatchAsync(MatchInfo match);
    }
}