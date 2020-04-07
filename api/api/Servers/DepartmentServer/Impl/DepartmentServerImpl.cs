using api.requests;
using api.responses;
using api.Servers.DepartmentServer.Interface;
using api.Servers.LogServer.Interface;
using common.Consts;
using common.DB.Interface;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Servers.DepartmentServer.Impl
{
    public class DepartmentServerImpl : BaseServiceImpl, IDepartmentServer
    {
        public DepartmentServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        public async Task<Result> AddDepartment(reqmodel<AddDepartmentModel> reqmodel)
        {
            bool has_parent = false;
            Result result = new Result { code = ErrorCodeConst.ERROR_100 };
            if (reqmodel.Data.department_parent != null)
            {
                string sql_exist_parent = g_sqlMaker.Select<t_department>(s => s.id)
                    .Where("id", "=", "@id")
                    .ToSQL();
                bool exist_parent_flag = await g_dbHelper.QueryAsync<int>(sql_exist_parent, new { id = reqmodel.Data.department_parent.Value }) != 0;
                if (!exist_parent_flag)
                {
                    result.code = ErrorCodeConst.ERROR_1024;
                    return result;
                }
                has_parent = true;
            }

            //上级部门
            string sql_insert = null;
            if (has_parent)
            {
                sql_insert = g_sqlMaker.Insert<t_department>(i => new { i.department_name, i.department_parent, i.state, i.status }).ToSQL();
            }
            else
            {
                sql_insert = g_sqlMaker.Insert<t_department>(i => new { i.department_name, i.state, i.status }).ToSQL();
            }

            t_department department_model = new t_department
            {
                department_name = reqmodel.Data.department_name,
                department_parent = reqmodel.Data.department_parent,
                state = (int)EnumState.Normal,
                status = (int)EnumStatus.Enable
            };
            bool insert_flag = await g_dbHelper.ExecAsync(sql_insert, department_model) > 0;
            if (!insert_flag)
            {
                result.code = ErrorCodeConst.ERROR_1018;
                return result;
            }

            result.status = ErrorCodeConst.ERROR_200;
            result.code = ErrorCodeConst.ERROR_1019;
            return result;
        }

        public async Task<Result> DeleteDepartment(reqmodel<int> reqmodel)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_1030, status = ErrorCodeConst.ERROR_403 };
            string sql_update = g_sqlMaker.Update<t_department>(u => new
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

        public async Task<Result<IEnumerable<DepartmentResult>>> GetAllDepartmentDeps()
        {
            //顶级部门
            string sql_select_top = g_sqlMaker.Select<t_department>().Where("department_parent", "=", "@department_parent").And("status", "=", "@status").And("state", "=", "@state").ToSQL();

            //获取下级部门
            async Task<IEnumerable<DepartmentResult>> GetChildDepartmentDeps(int? id)
            {
                IEnumerable<t_department> top_depart_list = await g_dbHelper.QueryListAsync<t_department>(sql_select_top, new { department_parent = id, status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
                List<DepartmentResult> result_list = new List<DepartmentResult>();
                foreach (var item in top_depart_list)
                {
                    result_list.Add(new DepartmentResult
                    {
                        id = item.id,
                        name = item.department_name,
                        parent = item.department_parent,
                        Childrens = await GetChildDepartmentDeps(item.id)
                    });
                }

                return result_list;
            }

            Result<IEnumerable<DepartmentResult>> result = new Result<IEnumerable<DepartmentResult>>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = await GetChildDepartmentDeps(null)
            };

            return result;
        }

        public async Task<Result<IEnumerable<DepartmentResult>>> GetAllDepartments()
        {
            string sql_select = g_sqlMaker.Select<t_department>().Where("status", "=", "@status").And("state", "=", "@state").ToSQL();

            List<DepartmentResult> result_list = new List<DepartmentResult>();
            List<t_department> depart_list = await g_dbHelper.QueryListAsync<t_department>(sql_select, new { status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
            foreach (var item in depart_list)
            {
                result_list.Add(new DepartmentResult(item));
            }

            Result<IEnumerable<DepartmentResult>> result = new Result<IEnumerable<DepartmentResult>>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = result_list
            };
            return result;
        }

        /// <summary>
        /// @xis 获取部门信息
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<t_department> GetDepartment(Func<t_department, dynamic> selector, int id)
        {
            string sql_select = g_sqlMaker.Select(selector).Where("id", "=", "@id").ToSQL();
            return await g_dbHelper.QueryAsync<t_department>(sql_select, new { id });
        }
    }
}
