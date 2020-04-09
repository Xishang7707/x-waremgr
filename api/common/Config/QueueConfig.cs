using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace common.Config
{
    /// <summary>
    /// 队列配置
    /// </summary>
    public class QueueConfig
    {
        public QueueConfig() { }

        public QueueConfig(JObject json)
        {
            RabbitMQ_HOST = json["rabbitmq_host"]?.ToString();
            RabbitMQ_Prefix = json["rabbitmq_prefix"]?.ToString();
            RabbitMQ_USER = json["rabbitmq_user"]?.ToString();
            RabbitMQ_PWD = json["rabbitmq_pwd"]?.ToString();
        }

        /// <summary>
        /// rabbit地址
        /// </summary>
        public string RabbitMQ_HOST { get; set; }

        /// <summary>
        /// rabbit后缀
        /// </summary>
        public string RabbitMQ_Prefix { get; set; }

        /// <summary>
        /// rabbit账号
        /// </summary>
        public string RabbitMQ_USER { get; set; }

        /// <summary>
        /// rabbit密码
        /// </summary>
        public string RabbitMQ_PWD { get; set; }
    }
}
