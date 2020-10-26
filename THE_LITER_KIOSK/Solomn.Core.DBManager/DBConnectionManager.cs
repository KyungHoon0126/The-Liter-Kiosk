using System.Data;

namespace THE_LITER_KIOSK.DataBase
{
    public abstract class DBConnectionManager
    {
        public abstract IDbConnection GetConnection();
    }
}
