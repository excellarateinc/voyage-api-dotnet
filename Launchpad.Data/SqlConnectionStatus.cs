using Launchpad.Data.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Launchpad.Core;


namespace Launchpad.Data
{
    public class SqlConnectionStatus : IDbConnectionStatus
    {
        private readonly string _connectionString;
        private readonly string _displayName;

        
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public SqlConnectionStatus(string connectionString, string displayName)
        {
            _connectionString = connectionString.ThrowIfNull(nameof(connectionString));
            _displayName = displayName.ThrowIfNull(nameof(displayName));
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public bool Test()
        {
            bool success = false;
            try
            {
                using(var connection = GetConnection())
                {
                    connection.Open();
                    connection.Close();
                    success = true;
                }
            }
            catch { }
            return success;
        }
    }
}
