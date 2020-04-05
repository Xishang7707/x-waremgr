using api.requests;
using api.responses;
using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.ProductServer.Interface
{
    /// <summary>
    /// 产品服务
    /// </summary>
    public interface IProductServer
    {
        /// <summary>
        /// @xis 添加产品 2020-3-29 08:32:18
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        //Task<Result> AddProduct(reqmodel<AddProductModel> reqmodel);

        /// <summary>
        /// @xis 删除产品 2020-3-29 08:32:18
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> DeleteProduct(reqmodel<int> reqmodel);

        /// <summary>
        /// @xis 产品是否存在 2020-3-29 08:49:13
        /// </summary>
        /// <param name="product_name"></param>
        /// <returns></returns>
        Task<bool> ExistProduct(string product_name);
    }
}
