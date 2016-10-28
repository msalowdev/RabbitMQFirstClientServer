using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQServer
{
    /// <summary>
    /// Class to encapsulate recieving messages from RabbitMQ
    /// </summary>
    public class RabbitConsumer : IDisposable
    {
        private const string HostName = "192.168.1.17";
        private const string UserName = "demouser";
        private const string Password = "test123";
        private const string QueueName = "SimpleOneWay";
        private const string ExchangeName = "SimpleOneWay";
        private const bool IsDurable = true;
        //The two below settings are just to illustrate how they can be used but we are not using them in
        //this sample as we will use the defaults
        private const string VirtualHost = "";
        private int Port = 0;

        public delegate void OnReceiveMessage(string message);

        public bool Enabled { get; set; }

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;


        /// <summary>
        /// Ctor with a key to lookup the configuration
        /// </summary>
        public RabbitConsumer()
        {
            DisplaySettings();
            _connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                _connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                _connectionFactory.Port = Port;

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);
        }
        /// <summary>
        /// Displays the rabbit settings
        /// </summary>
        private void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", HostName);
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", Password);
            Console.WriteLine("QueueName: {0}", QueueName);
            Console.WriteLine("ExchangeName: {0}", ExchangeName);
            Console.WriteLine("VirtualHost: {0}", VirtualHost);
            Console.WriteLine("Port: {0}", Port);
            Console.WriteLine("Is Durable: {0}", IsDurable);
        }
        /// <summary>
        /// Starts receiving a message from a queue
        /// </summary>
        public void Start()
        {
            var consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(QueueName, false, consumer);

            while (Enabled)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Serialize message
                var message = Encoding.Default.GetString(deliveryArgs.Body);

                Console.WriteLine("Message Recieved - {0}", message);
                _model.BasicAck(deliveryArgs.DeliveryTag, false);
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_model != null)
                _model.Dispose();
            if (_connection != null)
                _connection.Dispose();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}
