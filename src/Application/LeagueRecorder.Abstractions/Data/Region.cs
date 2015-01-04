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
        /// Returns the readable string for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        public static string GetReadableString(this Region region)
        {
            switch (region)
            {
                case Region.Korea:
                    return "Republic of Korea";
                case Region.NorthAmerica:
                    return "North America";
                case Region.EuropeWest:
                    return "Europe West";
                case Region.EuropeNordic:
                    return "Europe Nordic & ERast";
                case Region.Oceania:
                    return "Oceania";
                case Region.Brazil:
                    return "Brazil";
                case Region.LAS:
                    return "Latin America South";
                case Region.LAN:
                    return "Latin America North";
                case Region.Russia:
                    return "Russia";
                case Region.Turkey:
                    return "Turkey";
                default:
                    throw new ArgumentOutOfRangeException("region");
            }
        }
        /// <summary>
        /// Returns the abbreviation for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        public static string GetAbbreviation(this Region region)
        {
            switch (region)
            {
                case Region.Korea:
                    return "KR";
                case Region.NorthAmerica:
                    return "NA";
                case Region.EuropeWest:
                    return "EUW";
                case Region.EuropeNordic:
                    return "EUNE";
                case Region.Oceania:
                    return "OCE";
                case Region.Brazil:
                    return "BR";
                case Region.LAS:
                    return "LAS";
                case Region.LAN:
                    return "LAN";
                case Region.Russia:
                    return "RU";
                case Region.Turkey:
                    return "TR";
                default:
                    throw new ArgumentOutOfRangeException("region");
            }
        }
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