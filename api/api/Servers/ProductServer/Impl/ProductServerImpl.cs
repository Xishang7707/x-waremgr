using api.requests;
using api.responses;
using api.Servers.FactoryServer.Impl;
using api.Servers.FactoryServer.Interface;
using api.Servers.LogServer.Interface;
using api.Servers.ProductServer.Interface;
using common.Consts;
using common.DB.Interface;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.ProductServer.Impl
{
    public class ProductServerImpl : BaseServiceImpl, IProductServer
    {
        public ProductServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        public async Task<Result> DeleteProduct(reqmodel<int> reqmodel)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_100, status = ErrorCodeConst.ERROR_403 };
            string sql_update = g_sqlMaker.Update<t_product>(u => new { u.state }).Where($"id=@id and state={(int)EnumState.Normal}").ToSQL();
            t_product product_model = new t_product
            {
                id = reqmodel.Data,
                state = (int)EnumState.Delete
            };
            bool update_flag = await g_dbHelper.ExecAsync(sql_update, product_model) > 0;
            if (!update_flag)
            {
                result.code = ErrorCodeConst.ERROR_1034;
                return result;
            }

            result.status = ErrorCodeConst.ERROR_200;
            result.code = ErrorCodeConst.ERROR_1033;
            return result;
        }

        public async Task<bool> ExistProduct(string product_name)
        {
            string sql_select = g_sqlMaker.Select<t_product>(s => new
            {
                s.id
            })
            .Where("product_name=@product_name and state=@state")
            .ToSQL();
            t_product product = await g_dbHelper.QueryAsync<t_product>(sql_select, new { product_name, state = (int)EnumState.Normal });
            return product != null;
        }
    }
}
