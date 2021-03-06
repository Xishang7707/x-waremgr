﻿using common.SqlMaker.Interface.Base;
using System;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 删除
    /// </summary>
    public interface IDelete<T> : ISqlBase where T : new()
    {
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rel"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IWhere<T> Where(string key, string rel, object val);
    }
}
