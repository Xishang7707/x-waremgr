using api.requests;
using api.responses;
using common.SqlMaker.Interface;
using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.FactoryServer.Interface
{
    /// <summary>
    /// 供应商
    /// </summary>
    public interface IFactoryServer
    {
        /// <summary>
        /// @xis 添加供应商 2020-3-28 12:53:28
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> AddFactory(reqmodel<AddFactoryModel> reqmodel);

        /// <summary>
        /// @xis 删除供应商 2020-3-28 21:45:10
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> DeleteFactory(reqmodel<int> reqmodel);

        /// <summary>
        /// @xis 获取供应商信息
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<t_factory> GetFactoryById(Func<t_factory, dynamic> selector, int id);

        /// <summary>
        /// @xis 获取供应商信息
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<t_factory> GetFactoryByIdEnable(Func<t_factory, dynamic> selector, int id);

        /// <summary>
        /// @xis 搜索供货商 下拉提示
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> SearchFactoryDrop(reqmodel<SearchFactoryModel> reqmodel);

        /// <summary>
        /// @xis 搜索供应商
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> SearchFactoryByPaginer(reqmodel<SearchFactoryModel> reqmodel);
    }
}
