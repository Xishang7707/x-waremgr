using api.Servers.LogServer.Interface;
using api.Servers.PositionServer.Interface;
using common.DB.Interface;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<t_position>> GetSubordinatePositions(Func<t_position, dynamic> selector, int position)
        {
            List<t_position> position_list = (await GetPositionByParentId(f => new { f.id }, position)).ToList();

            if (position_list.Count == 0)
            {
                return position_list;
            }

            List<t_position> result_list = new List<t_position>();
            result_list.AddRange(position_list);
            foreach (var item in position_list)
            {
                IEnumerable<t_position> temp_list = await GetSubordinatePositions(selector, item.id);
                if (temp_list.Count() <= 0)
                {
                    continue;
                }
                result_list.AddRange(temp_list);
            }

            return result_list;
        }

        /// <summary>
        /// @xis 获取下属职位
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public async Task<List<t_position>> GetPositionByParentId(Func<t_position, dynamic> selector, int position)
        {
            string sql = g_sqlMaker.Select(selector).Where("position_parent", "=", "@parent_id").ToSQL();
            return await g_dbHelper.QueryListAsync<t_position>(sql, new { parent_id = position });
        }
    }
}
