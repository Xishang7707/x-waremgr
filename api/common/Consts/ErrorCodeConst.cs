using System;
using System.Collections.Generic;
using System.Text;

namespace common.Consts
{
    public class ErrorCodeConst
    {
        /// <summary>
        /// 错误码
        /// </summary>
        #region 服务器状态

        /// <summary>
        /// 失败
        /// </summary>
        public const string ERROR_100 = "100";

        /// <summary>
        /// 成功
        /// </summary>
        public const string ERROR_200 = "200";

        /// <summary>
        /// 请求被接受 但未被正常处理
        /// </summary>
        public const string ERROR_202 = "202";

        /// <summary>
        /// 拒绝处理
        /// </summary>
        public const string ERROR_400 = "400";

        /// <summary>
        /// 未授权访问
        /// </summary>
        public const string ERROR_401 = "401";

        /// <summary>
        /// 403 Forbidden
        /// </summary>
        public const string ERROR_403 = "403";

        /// <summary>
        /// 404 未找到
        /// </summary>
        public const string ERROR_404 = "404";

        /// <summary>
        /// 服务器错误
        /// </summary>
        public const string ERROR_500 = "500";

        #endregion

        #region 请求错误

        /// <summary>
        /// 未知请求
        /// </summary>
        public const string ERROR_600 = "600";

        /// <summary>
        /// 未登录
        /// </summary>
        public const string ERROR_1001 = "1001";

        /// <summary>
        /// 用户名不能为空
        /// </summary>
        public const string ERROR_1002 = "1002";

        /// <summary>
        /// 密码不能为空
        /// </summary>
        public const string ERROR_1003 = "1003";

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public const string ERROR_1004 = "1004";

        /// <summary>
        /// 用户名已存在
        /// </summary>
        public const string ERROR_1005 = "1005";

        /// <summary>
        /// 用户名在5-15位之间
        /// </summary>
        public const string ERROR_1006 = "1006";

        /// <summary>
        /// 密码在6-18位之间
        /// </summary>
        public const string ERROR_1007 = "1007";

        /// <summary>
        /// 登录成功
        /// </summary>
        public const string ERROR_1008 = "1008";

        /// <summary>
        /// 导航名称不能为空
        /// </summary>
        public const string ERROR_1009 = "1009";

        /// <summary>
        /// 唯一key不能为空
        /// </summary>
        public const string ERROR_1010 = "1010";

        /// <summary>
        /// 启用状态错误
        /// </summary>
        public const string ERROR_1011 = "1011";

        /// <summary>
        /// 导航名称最长为15字符
        /// </summary>
        public const string ERROR_1012 = "1012";

        /// <summary>
        /// 导航key最长为30个字符
        /// </summary>
        public const string ERROR_1013 = "1013";

        /// <summary>
        /// 导航上级Id错误
        /// </summary>
        public const string ERROR_1014 = "1014";

        /// <summary>
        /// 链接最长为250个字符
        /// </summary>
        public const string ERROR_1015 = "1015";

        /// <summary>
        /// icon样式最长为50个字符
        /// </summary>
        public const string ERROR_1016 = "1016";

        /// <summary>
        /// 导航备注最多为50个字符
        /// </summary>
        public const string ERROR_1017 = "1017";

        /// <summary>
        /// 添加失败
        /// </summary>
        public const string ERROR_1018 = "1018";

        /// <summary>
        /// 添加成功
        /// </summary>
        public const string ERROR_1019 = "1019";

        /// <summary>
        /// 职位信息错误
        /// </summary>
        public const string ERROR_1020 = "1020";

        /// <summary>
        /// 姓名不能为空
        /// </summary>
        public const string ERROR_1021 = "1021";

        /// <summary>
        /// 登录失败
        /// </summary>
        public const string ERROR_1022 = "1022";

        /// <summary>
        /// 部门名称不能为空
        /// </summary>
        public const string ERROR_1023 = "1023";

        /// <summary>
        /// 上级部门不存在
        /// </summary>
        public const string ERROR_1024 = "1024";

        /// <summary>
        /// 供应商名称不能为空
        /// </summary>
        public const string ERROR_1025 = "1025";

        /// <summary>
        /// 供应商手机不能为空
        /// </summary>
        public const string ERROR_1026 = "1026";

        /// <summary>
        /// 供应商联系人不能为空
        /// </summary>
        public const string ERROR_1027 = "1027";

        /// <summary>
        /// 手机号码格式错误
        /// </summary>
        public const string ERROR_1028 = "1028";

        /// <summary>
        /// 操作成功
        /// </summary>
        public const string ERROR_1029 = "1029";

        /// <summary>
        /// 操作失败
        /// </summary>
        public const string ERROR_1030 = "1030";

        /// <summary>
        /// 产品已存在
        /// </summary>
        public const string ERROR_1031 = "1031";

        /// <summary>
        /// 产品不存在
        /// </summary>
        public const string ERROR_1032 = "1032";

        /// <summary>
        /// 删除成功
        /// </summary>
        public const string ERROR_1033 = "1033";

        /// <summary>
        /// 删除失败
        /// </summary>
        public const string ERROR_1034 = "1034";

        /// <summary>
        /// 无权限操作
        /// </summary>
        public const string ERROR_1035 = "1035";

        /// <summary>
        /// 成功提交申请
        /// </summary>
        public const string ERROR_1036 = "1036";

        /// <summary>
        /// 提交申请失败
        /// </summary>
        public const string ERROR_1037 = "1037";

        /// <summary>
        /// 订单不存在
        /// </summary>
        public const string ERROR_1038 = "1038";

        /// <summary>
        /// 订单已经被审核完成
        /// </summary>
        public const string ERROR_1039 = "1039";

        /// <summary>
        /// 无权限操作
        /// </summary>
        public const string ERROR_1040 = "1040";

        /// <summary>
        /// 订单已经被驳回
        /// </summary>
        public const string ERROR_1041 = "1041";

        /// <summary>
        /// 请添加入库的产品
        /// </summary>
        public const string ERROR_1042 = "1042";

        /// <summary>
        /// 入库产品数量必须大于0
        /// </summary>
        public const string ERROR_1043 = "1043";

        /// <summary>
        /// 产品名称不能为空
        /// </summary>
        public const string ERROR_1044 = "1044";

        /// <summary>
        /// 产品供货商信息错误
        /// </summary>
        public const string ERROR_1045 = "1045";

        /// <summary>
        /// 产品名称最长为30个字符
        /// </summary>
        public const string ERROR_1046 = "1046";

        /// <summary>
        /// 数据格式错误
        /// </summary>
        public const string ERROR_1047 = "1047";

        /// <summary>
        /// 订单号不能为空
        /// </summary>
        public const string ERROR_1048 = "1048";

        /// <summary>
        /// 备注最长100个字符
        /// </summary>
        public const string ERROR_1049 = "1049";
        #endregion
    }
}
