using api.Externs;
using api.responses;
using api.Servers.AuditServer.Interface;
using api.Servers.LogServer.Interface;
using api.Servers.PositionServer.Impl;
using api.Servers.PositionServer.Interface;
using api.Servers.UserServer.Impl;
using api.Servers.UserServer.Interface;
using common;
using common.DB.Interface;
using models.db_models;
using models.enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.AuditServer.Impl
{
    /// <summary>
    /// 审批
    /// </summary>
    public class AuditServerImpl : BaseServiceImpl, IAuditServer
    {
        public AuditServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        public async Task<List<ApplyProcess>> GetApplyLogByOrderSnAsync(string order_sn)
        {
            string sql = g_sqlMaker.Select<t_apply_log>().Where("order_sn", "=", "@order_sn").ToSQL();
            List<t_apply_log> apply_log_list = await g_dbHelper.QueryListAsync<t_apply_log>(sql, new { order_sn });
            IPositionServer positionServer = new PositionServerImpl(g_dbHelper, g_logServer);
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);
            List<ApplyProcess> apply_list = new List<ApplyProcess>();
            foreach (var item in apply_log_list)
            {
                t_position position_model = await positionServer.GetPosition(s => new { s.position_name }, item.position_id);
                t_user user_model = await userServer.GetUserById(s => new { s.real_name }, item.user_id);
                apply_list.Add(new ApplyProcess
                {
                    audit_status = item.apply_status,
                    audit_time = item.add_time.Value.ToString("yyyy-MM-dd hh:mm"),
                    remark = item.remark,
                    position_name = position_model.position_name,
                    auditer = user_model.real_name
                });
            }

            return apply_list;
        }


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
        public int? GetNextApplyer(EnumOrderType _ot, int _depart, int _cur_position)
        {
            JObject process_json = CommonConfig.ProcessConfig[_ot.ToString()] as JObject;
            if (process_json == null)
            {
                return null;
            }

            List<int> process_list = JsonConvert.DeserializeObject<List<int>>(process_json[$"d_{_depart}"].ToString());//流程列表
            if (process_list == null)
            {
                return null;
            }

            int cur_index = process_list.IndexOf(_cur_position);
            //不存在或达末尾
            if (cur_index == -1 || cur_index + 1 >= process_list.Count)
            {
                return null;
            }

            return process_list[cur_index + 1];
        }

        /// <summary>
        /// @xis 审批进度 开始/进行中/末尾 EnumApplyStepFlag
        /// </summary>
        /// <returns></returns>
        public EnumApplyStepFlag GetApplyStepFlag(EnumOrderType _ot, int _depart, int _cur_position)
        {
            List<int> process_list = JsonConvert.DeserializeObject<List<int>>(CommonConfig.ProcessConfig[_ot.ToString()][$"d_{_depart}"].ToString());
            int cur_index = process_list.IndexOf(_cur_position);
            if (cur_index == 0)
            {
                return EnumApplyStepFlag.Start;
            }
            if (cur_index + 1 != process_list.Count)
            {
                return EnumApplyStepFlag.Progress;
            }

            return EnumApplyStepFlag.End;
        }

        public IEnumerable<int> GetApplyList(EnumOrderType _ot, int _depart, int _start_position)
        {
            JObject process_json = CommonConfig.ProcessConfig[_ot.ToString()] as JObject;
            if (process_json == null)
            {
                return null;
            }
            List<int> process_list = JsonConvert.DeserializeObject<List<int>>(process_json[$"d_{_depart}"].ToString());//流程列表

            List<int> result_list = new List<int>();
            for (int i = process_list.IndexOf(_start_position) + 1; i < process_list.Count; i++)
            {
                result_list.Add(process_list[i]);
            }
            return result_list;
        }

        public int GetApplyIndex(EnumOrderType _ot, int _depart, int _start_position, int _cur_audited_position)
        {
            JObject process_json = CommonConfig.ProcessConfig[_ot.ToString()] as JObject;
            if (process_json == null)
            {
                return -1;
            }
            List<int> process_list = GetApplyList(_ot, _depart, _start_position).ToList();//流程列表

            int index = process_list.IndexOf(_cur_audited_position);
            if (index == -1)
            {
                return 0;
            }

            if (process_list.Count == index + 1)
            {
                return index;
            }
            return index + 1;
        }

        public async Task<List<ApplyProcess>> GetApplyLogByOrderSnAsync(EnumOrderType _ot, string order_sn, int _depart, int _start_position)
        {
            string sql = g_sqlMaker.Select<t_apply_log>().Where("order_sn", "=", "@order_sn").ToSQL();
            List<t_apply_log> apply_log_list = await g_dbHelper.QueryListAsync<t_apply_log>(sql, new { order_sn });
            IPositionServer positionServer = new PositionServerImpl(g_dbHelper, g_logServer);
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);
            List<ApplyProcess> apply_list = new List<ApplyProcess>();
            IEnumerable<int> apply_proc_list = GetApplyList(_ot, _depart, _start_position);
            foreach (var item in apply_proc_list)
            {
                t_apply_log apply_log_model = apply_log_list.FirstOrDefault(f => f.position_id == item);
                t_position position_model = await positionServer.GetPosition(s => new { s.position_name }, item);

                if (apply_log_model != null)
                {
                    t_user user_model = await userServer.GetUserById(s => new { s.real_name }, apply_log_model.user_id);
                    apply_list.Add(new ApplyProcess
                    {
                        audit_status = apply_log_model.apply_status,
                        audit_status_desc = ((EnumAuditStatus)apply_log_model.apply_status).GetDesc(),
                        audit_time = apply_log_model.add_time.Value.ToString("yyyy-MM-dd hh:mm"),
                        remark = apply_log_model.remark,
                        position_name = position_model.position_name,
                        auditer = user_model.real_name,
                    });
                }
                else
                {
                    apply_list.Add(new ApplyProcess
                    {
                        position_name = position_model.position_name
                    });
                }
            }

            return apply_list;
        }
    }
}
