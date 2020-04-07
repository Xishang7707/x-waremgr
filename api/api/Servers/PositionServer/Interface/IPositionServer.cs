using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.PositionServer.Interface
{
    /// <summary>
    /// 职位
    /// </summary>
    public interface IPositionServer
    {
        /// <summary>
        /// 插入职位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(t_position model);

        /// <summary>
        /// @xis 职位是否存在 2020-3-24 08:22:06
        /// </summary>
        /// <param name="position_id"></param>
        /// <returns></returns>
        Task<bool> ExistPositionAsync(int position_id);

        /// <summary>
        /// @xis 获取职位信息
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<t_position> GetPosition(Func<t_position, dynamic> selector, int id);
    }
}
