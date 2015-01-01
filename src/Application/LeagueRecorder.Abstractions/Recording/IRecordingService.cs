﻿using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using NeverNull;

namespace LeagueRecorder.Abstractions.Recording
{
    public interface IRecordingService : IService
    {
        Task<MatchInfo> GetCurrentMatchInfoFromUserAsync(User user);

        Task<bool> RequestRecordingOfMatchAsync(MatchInfo match);
    }
}