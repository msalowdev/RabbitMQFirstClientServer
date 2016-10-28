using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ Message Sender");
            Console.WriteLine();
            Console.WriteLine();

            var messageCount = 0;
            var sender = new RabbitSender();

            Console.WriteLine("Press enter key to send a message");
            int max = 100000;
            int current = 0;
            while (current < max)
            {
                //var key = Console.ReadKey();
                //if (key.Key == ConsoleKey.Q)
                //    break;

                //if (key.Key == ConsoleKey.Enter)
                //{
                //    var message = string.Format("Message: {0}", messageCount);
                //    Console.WriteLine(string.Format("Sending - {0}", message));
                //    sender.Send(message);
                //    messageCount++;
                //}

                sender.Send(current.ToString());
                current++;
            }

            Console.ReadLine();
        }
    }
}
