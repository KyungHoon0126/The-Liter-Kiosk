using System.Data;

namespace TheLiter.Core.DBManager
{
    public abstract class DBConnectionManager
    {
        public abstract IDbConnection GetConnection();
    }
}
