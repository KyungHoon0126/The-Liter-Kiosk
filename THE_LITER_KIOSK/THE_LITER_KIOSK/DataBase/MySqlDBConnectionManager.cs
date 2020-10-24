using MySql.Data.MySqlClient;
using System.Data;

namespace THE_LITER_KIOSK.DataBase
{
    public class MySqlDBConnectionManager : DBConnectionManager
    {
        private readonly string DATA_BASE_URL = $"SERVER=localhost;DATABASE=;UID=root;PASSWORD=;allow user variables=true";

        public override IDbConnection GetConnection()
        {
            IDbConnection db = new MySqlConnection(DATA_BASE_URL);
            return db;
        }
    }
}
