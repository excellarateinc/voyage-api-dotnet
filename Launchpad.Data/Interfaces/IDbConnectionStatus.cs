namespace Launchpad.Data.Interfaces
{

    /// <summary>
    /// Represents an interface that tests the ability to connect to a database
    /// </summary>
    public interface IDbConnectionStatus
    {
        /// <summary>
        /// Tests if a connection can be estabished to a database
        /// </summary>
        /// <returns></returns>
        bool Test();
        string DisplayName { get; }

     
    }
}
