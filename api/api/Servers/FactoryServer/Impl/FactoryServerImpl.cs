using api.requests;
using api.responses;
using api.Servers.FactoryServer.Interface;
using api.Servers.LogServer.Interface;
using common.Consts;
using common.DB.Interface;
using common.SqlMaker.Interface;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Servers.FactoryServer.Impl
{
    public class FactoryServerImpl : BaseServiceImpl, IFactoryServer
    {
        public FactoryServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        public async Task<Result> AddFactory(reqmodel<AddFactoryModel> reqmodel)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_1018, status = ErrorCodeConst.ERROR_403 };
            if (!Regex.IsMatch(reqmodel.Data.factory_tel, @"^\d{11}$"))
            {
                result.code = ErrorCodeConst.ERROR_1028;
                return result;
            }

            string sql_insert = g_sqlMaker.Insert<t_factory>(i =>
            new
            {
                i.factory_name,
                i.factory_person_name,
                i.factory_tel,
                i.status,
                i.state,
            }).ToSQL();

            t_factory factory_model = new t_factory
            {
                factory_name = reqmodel.Data.factory_name,
                factory_tel = reqmodel.Data.factory_tel,
                factory_person_name = reqmodel.Data.factory_person_name,
                status = reqmodel.Data.status.GetValueOrDefault((int)EnumStatus.Enable),
                state = (int)EnumState.Normal
            };
            bool insert_flag = await g_dbHelper.ExecAsync(sql_insert, factory_model) > 0;
            if (!insert_flag)
            {
                result.code = ErrorCodeConst.ERROR_1018;
                return result;
            }

            result.status = ErrorCodeConst.ERROR_200;
            result.code = ErrorCodeConst.ERROR_1019;
            return result;
        }

        public async Task<Result> DeleteFactory(reqmodel<int> reqmodel)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_1030, status = ErrorCodeConst.ERROR_403 };
            string sql_update = g_sqlMaker.Update<t_factory>(u => new
            {
                u.state,
            }).Where("id", "=", "@id").And("state", "=", "@state")
                .ToSQL();

            bool disable_flag = await g_dbHelper.ExecAsync(sql_update, new { id = reqmodel.Data, state = (int)EnumState.Normal }) > 0;
            if (!disable_flag)
            {
                return result;
            }

            result.code = ErrorCodeConst.ERROR_1029;
            result.status = ErrorCodeConst.ERROR_200;

            return result;
        }

        public async Task<t_factory> GetFactoryById(Func<t_factory, dynamic> selector, int id)
        {
            string sql_select_factory = g_sqlMaker.Select(selector).Where("id", "=", "@id").ToSQL();
            return await g_dbHelper.QueryAsync<t_factory>(sql_select_factory, new { id });
        }

        public async Task<t_factory> GetFactoryByIdEnable(Func<t_factory, dynamic> selector, int id)
        {
            string sql_select_factory = g_sqlMaker.Select(selector).Where("id", "=", "@id").And("state", "=", "@state").And("status", "=", "@status").ToSQL();
            return await g_dbHelper.QueryAsync<t_factory>(sql_select_factory, new { id, state = (int)EnumState.Normal, status = (int)EnumStatus.Enable });
        }

        /// <summary>
        /// @xis 名称模糊查询供货商
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IEnumerable<t_factory>> GetFactoryByVagueName(Func<t_factory, dynamic> selector, string name)
        {
            string sql = g_sqlMaker.Select(selector).Where("factory_name", "like", "@factory_name").And("status", "=", "@status").And("state", "=", "@state").ToSQL();
            return await g_dbHelper.QueryListAsync<t_factory>(sql, new { factory_name = $"%{name}%", status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
        }

        /// <summary>
        /// @xis 名称模糊查询供货商
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="name"></param>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        public async Task<PaginerData<List<SearchFactoryResult>>> SearchFactoryByVagueName(string name, int page_index, int page_size = 15)
        {
            IWhere<t_factory> where_data = g_sqlMaker.Select<t_factory>(s => new { s.id, s.factory_name, s.factory_person_name, s.factory_tel }).Where();
            IWhere<t_factory> where_count = g_sqlMaker.Select<t_factory>().Count().Where();
            if (!string.IsNullOrWhiteSpace(name))
            {
                where_data.And("name", "like", "@name");
                where_count.And("name", "like", "@name");
            }

            where_data.And("status", "=", "@status").And("state", "=", "@state").OrderByDesc("add_time").Pager(page_index, page_size);
            where_count.And("status", "=", "@status").And("state", "=", "@state");
            List<t_factory> data_list = await g_dbHelper.QueryListAsync<t_factory>(where_data.ToSQL(), new { factory_name = $"%{name}%", status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
            int total = await g_dbHelper.QueryAsync<int>(where_count.ToSQL(), new { factory_name = $"%{name}%", status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });

            List<SearchFactoryResult> list = new List<SearchFactoryResult>();
            foreach (var item in data_list)
            {
                list.Add(new SearchFactoryResult
                {
                    id = item.id,
                    factory_name = item.factory_name,
                    factory_person_name = item.factory_person_name,
                    factory_tel = item.factory_tel
                });
            }
            PaginerData<List<SearchFactoryResult>> paginer_data = new PaginerData<List<SearchFactoryResult>>
            {
                Data = list,
                page_index = page_index,
                page_size = page_size,
                total = total
            };
            paginer_data.page_total = (paginer_data.total % page_size > 0 ? 1 : 0) + paginer_data.total / page_size;
            return paginer_data;
        }

        public async Task<Result> SearchFactoryByPaginer(reqmodel<SearchFactoryModel> reqmodel)
        {
            Result<PaginerData<List<SearchFactoryResult>>> result = new Result<PaginerData<List<SearchFactoryResult>>> { status = ErrorCodeConst.ERROR_200, code = ErrorCodeConst.ERROR_200 };
            result.data = await SearchFactoryByVagueName(reqmodel.Data.name, reqmodel.Data.page_index, reqmodel.Data.page_size);
            return result;
        }

        public async Task<Result> SearchFactoryDrop(reqmodel<SearchFactoryModel> reqmodel)
        {
            Result<List<KVItemResult<int, string>>> result = new Result<List<KVItemResult<int, string>>> { status = ErrorCodeConst.ERROR_200, code = ErrorCodeConst.ERROR_200 };
            IEnumerable<t_factory> list = await GetFactoryByVagueName(s => new { s.id, s.factory_name }, reqmodel.Data.name);
            result.data = new List<KVItemResult<int, string>>();
            foreach (var item in list)
            {
                result.data.Add(new KVItemResult<int, string>
                {
                    key = item.id,
                    value = item.factory_name
                });
            }

            return result;
        }
    }
}
