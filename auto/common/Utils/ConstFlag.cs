using System;

namespace common.Utils
{
    public class ConstFlag
    {
        /// <summary>
        /// Token
        /// </summary>
        public const string TOKEN = "Token";

        /// <summary>
        /// 默认语言zh-cn
        /// </summary>
        public const string LANG_DEFAULT = "zh-cn";

        /// <summary>
        /// 语言标记
        /// </summary>
        public const string LANG_FLAG = "lang";

        /// <summary>
        /// 用户登录过期时间4小时
        /// </summary>
        public const int UserExpireTime = 3600 * 4;
    }
}
