using System;
using System.Collections;
using System.Collections.Generic;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface IAutoRecorder
    {
        /// <summary>
        /// Starts to record all matches of the specified <paramref name="players"/>.
        /// </summary>
        /// <param name="players">The players.</param>
        IDisposable StartRecordingPlayerMatches(Player[] players);
    }
}