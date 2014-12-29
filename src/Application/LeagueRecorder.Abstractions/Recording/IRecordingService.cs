using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using NeverNull;

namespace LeagueRecorder.Abstractions.Recording
{
    public interface IRecordingService : IService
    {
        Task<Option<MatchInfo>> GetCurrentGameIdFromUserAsync(User user);

        Task<Option<bool>> RequestRecordingOfGameAsync(MatchInfo match);

        Task<Option<string>> GetCommandsToStartSpectatingAsync(MatchInfo match);
    }
}