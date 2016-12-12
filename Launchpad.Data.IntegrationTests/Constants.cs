namespace Launchpad.Data.IntegrationTests
{
    public static class Constants
    {
        // Collection name for all integration tests
        // Integration tests cannot run in parallel otherwise multiple transactions can be 
        // initiated triggering distributed transactions error
        public const string CollectionName = "LP-IntegrationTests";
    }
}
