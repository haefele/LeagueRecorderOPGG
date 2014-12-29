using System;

namespace LeagueRecorder.Abstractions.Data
{
    public enum Region
    {
        Korea,
        NorthAmerica,
        EuropeWest,
        EuropeNordic,
        Oceania,
        Brazil,
        // ReSharper disable once InconsistentNaming
        LAS,
        // ReSharper disable once InconsistentNaming
        LAN,
        Russia,
        Turkey
    }

    public static class RegionExtensions
    {
        /// <summary>
        /// Returns the base URI for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        public static string GetBaseUri(this Region region)
        {
            string prefix = GetPrefix(region);
            return string.Format("http://{0}.op.gg", prefix);
        }
        /// <summary>
        /// Returns the URI prefix for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        private static string GetPrefix(Region region)
        {
            switch (region)
            {
                case Region.Korea:
                    return "www";
                case Region.NorthAmerica:
                    return "na";
                case Region.EuropeWest:
                    return "euw";
                case Region.EuropeNordic:
                    return "eune";
                case Region.Oceania:
                    return "oce";
                case Region.Brazil:
                    return "br";
                case Region.LAS:
                    return "las";
                case Region.LAN:
                    return "lan";
                case Region.Russia:
                    return "ru";
                case Region.Turkey:
                    return "tr";
                default:
                    throw new ArgumentOutOfRangeException("region");
            }
        }
    }
}