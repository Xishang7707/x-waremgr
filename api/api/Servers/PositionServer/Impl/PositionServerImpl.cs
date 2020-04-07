using api.Servers.LogServer.Interface;
using api.Servers.PositionServer.Interface;
using common.DB.Interface;
using models.db_models;
using models.enums;
using System;
using System.Threading.Tasks;

namespace api.Servers.PositionServer.Impl
{
    public class PositionServerImpl : BaseServiceImpl, IPositionServer
    {
        public PositionServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }
        public int Insert(t_position model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistPositionAsync(int position_id)
        {
            string sql_position_exist = g_sqlMaker.Select<t_position>(s => new
            {
                s.id
            })
            .Where("id", "=", "@id")
            .And("state", "=", (int)EnumState.Normal)
            .ToSQL();

            t_position model = await g_dbHelper.QueryAsync<t_position>(sql_position_exist, new { id = position_id, state = (int)EnumState.Normal });

            return model != null;
        }

        public async Task<t_position> GetPosition(Func<t_position, dynamic> selector, int id)
        {
            string sql_select = g_sqlMaker.Select(selector).Where("id", "=", "@id").ToSQL();
            return await g_dbHelper.QueryAsync<t_position>(sql_select, new { id });
        }
    }
}
