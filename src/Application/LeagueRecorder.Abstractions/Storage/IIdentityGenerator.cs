namespace LeagueRecorder.Abstractions.Storage
{
    public interface IIdentityGenerator
    {
        /// <summary>
        /// Generates a new identity.
        /// </summary>
        string Generate();
    }
}