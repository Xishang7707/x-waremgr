using common.Config.Enums;
using common.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace common.Config
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig
    {
        public DBConfig() { }
        public DBConfig(JArray config_arr)
        {
            foreach (var item in config_arr)
            {
                DBList.Add(new DBConfig()
                {
                    Tag = item["tag"]?.ToString(),
                    Conn = Common.AESDecrypt(item["conn"]?.ToString(), CommonConfig.GGUID),
                    DbType = (EnumDBType)int.Parse(item["db_type"].ToString()),
                    ActionType = (EnumDBActionType)int.Parse(item["action_type"].ToString()),
                });
            }
            DBConfig db = Get("main");
            this.Tag = db.Tag;
            this.Conn = db.Conn;
            this.DbType = db.DbType;
            this.ActionType = db.ActionType;
        }

        /// <summary>
        /// 连接标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Conn { get; private set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public EnumDBType DbType { get; private set; }

        /// <summary>
        /// 数据库作用类型
        /// </summary>
        public EnumDBActionType ActionType { get; private set; }

        /// <summary>
        /// 超时8秒
        /// </summary>
        public int TimeOut { get; private set; } = 8;

        /// <summary>
        /// 数据库列表
        /// </summary>
        private static List<DBConfig> DBList { get; set; } = new List<DBConfig>();

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db_type">数据库类型</param>
        /// <param name="action_type">数据库作用类型</param>
        /// <returns></returns>
        public DBConfig Get(EnumDBType db_type, EnumDBActionType action_type)
        {
            return DBList.FirstOrDefault(f => f.DbType == db_type && f.ActionType == action_type);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="_tag">数据库标签</param>
        /// <returns></returns>
        public DBConfig Get(string _tag)
        {
            return DBList.FirstOrDefault(f => f.Tag == _tag);
        }
    }
}
