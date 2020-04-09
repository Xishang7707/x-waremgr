using System;

namespace common.Logger
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public static class LogHelper
    {
        static NLog.Logger _logger;
        static NLog.Logger Instance
        {
            get
            {
                if (_logger == null)
                {
                    _logger = NLog.LogManager.GetCurrentClassLogger();
                }
                return _logger;
            }
        }

        public static void Debug(string msg, params object[] args)
        {
            Instance.Debug(msg, args);
        }

        public static void Debug(string msg)
        {
            Instance.Debug(msg);
        }

        public static void Info(string msg, params object[] args)
        {
            Instance.Info(msg, args);
        }

        public static void Info(string msg)
        {
            Instance.Info(msg);
        }

        public static void Trace(string msg, params object[] args)
        {
            Instance.Trace(msg, args);
        }

        public static void Trace(Exception err, string msg)
        {
            Instance.Trace(err, msg);
        }

        public static void Error(string msg, params object[] args)
        {
            Instance.Error(msg, args);
        }

        public static void Error(Exception err, string msg)
        {
            Instance.Error(err, msg);
        }
    }
}
