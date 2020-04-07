using api.responses;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.AuditServer.Interface
{
    /// <summary>
    /// 审批
    /// </summary>
    public interface IAuditServer
    {
        /// <summary>
        /// 获取订单审批记录
        /// </summary>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        Task<List<ApplyProcess>> GetApplyLogByOrderSnAsync(string order_sn);

        /// <summary>
        /// 获取订单审批记录
        /// </summary>
        /// <param name="_ot">订单类型</param>
        /// <param name="order_sn">订单号</param>
        /// <param name="_depart">部门id</param>
        /// <param name="_start_position">职位id</param>
        /// <returns></returns>
        Task<List<ApplyProcess>> GetApplyLogByOrderSnAsync(EnumOrderType _ot, string order_sn, int _depart, int _start_position);

        /// <summary>
        /// @xis 获取下一个审批者
        /// </summary>
        /// <param name="_ot">订单类型</param>
        /// <param name="_depart">申请部门</param>
        /// <param name="_cur_position">当前已审批的职位</param>
        /// <returns>
        /// null：获取失败
        /// -1：已达末尾
        /// </returns>
        int? GetNextApplyer(EnumOrderType _ot, int _depart, int _cur_position);

        /// <summary>
        /// @xis 审批进度 开始/进行中/末尾 EnumApplyStepFlag
        /// </summary>
        /// <param name="_ot">订单类型</param>
        /// <param name="_depart">部门</param>
        /// <param name="_cur_position">当前已审批职位</param>
        /// <returns></returns>
        EnumApplyStepFlag GetApplyStepFlag(EnumOrderType _ot, int _depart, int _cur_position);

        /// <summary>
        /// @xis 获取订单审批职位
        /// </summary>
        /// <param name="_ot">订单类型</param>
        /// <param name="_depart">部门</param>
        /// <param name="_start_position">起始职位</param>
        /// <returns></returns>
        IEnumerable<int> GetApplyList(EnumOrderType _ot, int _depart, int _start_position);
    }
}
