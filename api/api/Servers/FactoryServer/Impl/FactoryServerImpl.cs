using api.requests;
using api.responses;
using api.Servers.FactoryServer.Interface;
using api.Servers.LogServer.Interface;
using common.Consts;
using common.DB.Interface;
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
    }
}
