using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace THE_LITER_KIOSK.DataBase
{
    public class DBManager<T>
    {
        public async Task IndexSortSqlAsync(IDbConnection conn, string sql)
        {
            await SqlMapper.QueryAsync(conn, sql);
        }

        public async Task<List<T>> GetListAsync(IDbConnection conn, string sql, string search, IDbTransaction tran = null)
        {
            return (await SqlMapper.QueryAsync<T>(conn, sql, new { search = search })).ToList();
        }

        public async Task<T> GetSingleDataAsync(IDbConnection conn, string sql, string search, IDbTransaction tran = null)
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(conn, sql, new { search = search });
        }

        public async Task<int> InsertAsync(IDbConnection conn, string sql, object param, IDbTransaction tran = null)
        {
            return await SqlMapper.ExecuteAsync(conn, sql, param, tran);
        }

        public async Task<int> UpdateAsync(IDbConnection conn, string sql, object param, IDbTransaction tran = null)
        {
            return await SqlMapper.ExecuteAsync(conn, sql, param, tran);
        }

        public async Task<int> DeleteAsync(IDbConnection conn, string sql, object param, IDbTransaction tran = null)
        {
            return await SqlMapper.ExecuteAsync(conn, sql, param, tran);
        }
    }
}
