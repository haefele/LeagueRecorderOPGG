using System;
using LeagueRecorder.Abstractions.Storage;

namespace LeagueRecorder.Windows.Storage
{
    public class IdentityGenerator : IIdentityGenerator
    {
        /// <summary>
        /// Generates a new identity.
        /// </summary>
        public string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}