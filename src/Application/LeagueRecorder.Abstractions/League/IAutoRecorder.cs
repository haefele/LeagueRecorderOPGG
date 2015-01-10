using System;
using System.Collections;
using System.Collections.Generic;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface IAutoRecorder
    {
        IDisposable StartRecordingPlayerMatches(Player[] players);
    }
}