namespace LeagueRecorder.Abstractions.Storage
{
    public interface IIdentityGenerator : IService
    {
        /// <summary>
        /// Generates a new identity.
        /// </summary>
        string Generate();
    }
}