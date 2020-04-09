using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace common.RabbitMQ
{
    public class QueueDataBase
    {

    }

    public class LogData : QueueDataBase
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 日志数据 json
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 产生时间
        /// </summary>
        public DateTime make_time { get; set; }
    }

    public class RabbitServer
    {
        private static RabbitServer _instalce;
        public static RabbitServer Instance { get { if (_instalce == null) new RabbitServer(); return _instalce; } }
        private IConnection connection;

        public RabbitServer()
        {
            Connect();
            _instalce = this;
        }

        private void Connect()
        {
            ConnectionFactory fac = new ConnectionFactory
            {
                HostName = GConfig.QueueConfig.RabbitMQ_HOST,
                UserName = GConfig.QueueConfig.RabbitMQ_USER,
                Password = GConfig.QueueConfig.RabbitMQ_PWD,
            };
            connection = fac.CreateConnection();
        }

        /// <summary>
        /// 创建管道
        /// </summary>
        /// <param name="name"></param>
        public IModel CreateQueue(string name)
        {
            var channel = connection.CreateModel();
            channel.QueueDeclare($"{name}{GConfig.QueueConfig.RabbitMQ_Prefix}", false, false, false, null);
            return channel;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="name"></param>
        public IModel CreateConsumer<T>(string name, Func<T, bool> func) where T : QueueDataBase
        {
            var channel = CreateQueue(name);
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
              {
                  var message = Encoding.UTF8.GetString(ea.Body);
                  bool flag = func(JsonConvert.DeserializeObject<T>(message));
                  //确认该消息已被消费
                  channel.BasicAck(ea.DeliveryTag, false);
              };
            //启动消费者
            channel.BasicConsume($"{name}{GConfig.QueueConfig.RabbitMQ_Prefix}", false, consumer);
            return channel;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void SendMessage<T>(string name, T msg) where T : QueueDataBase
        {
            var channel = CreateQueue(name);
            string str = JsonConvert.SerializeObject(msg);
            var body = Encoding.UTF8.GetBytes(str);
            channel.BasicPublish(exchange: "",
                                    routingKey: $"{name}{GConfig.QueueConfig.RabbitMQ_Prefix}",
                                    basicProperties: null,
                                    body: body);
        }
    }
}
