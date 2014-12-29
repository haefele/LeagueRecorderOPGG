using System.Threading.Tasks;
using NeverNull;

namespace LeagueRecorder.Abstractions.Recording
{
    public interface ISpectatorService : IService
    {
        Task ExecuteSpectatorCommandsAsync(string commands);
    }
}