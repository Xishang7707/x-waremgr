using api.requests;
using api.responses;
using models.db_models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace api.Servers.UserServer.Interface
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserServer
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> AddUserAsync(reqmodel<RegisterModel> reqmodel);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> LoginAsync(reqmodel<LoginModel> reqmodel);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> LoginOutAsync(reqmodel reqmodel);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> GetUserInfoAsync(reqmodel reqmodel);

        /// <summary>
        /// token 续期/记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> TokenRenewalAsync(string token, LoginResult user);

        /// <summary>
        /// @xis 获取用户
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<t_user> GetUserById(Func<t_user, dynamic> selector, int id);
    }
}
