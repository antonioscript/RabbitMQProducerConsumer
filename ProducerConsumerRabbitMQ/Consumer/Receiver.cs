using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Producer;
public class Receiver
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var chanel = connection.CreateModel())
        {
            chanel.QueueDeclare("BasicTest", false, false, false, null);
            
            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message {message}...");
            };

            chanel.BasicConsume("BasicTest", true, consumer);

            Console.WriteLine("Press [Ente] to exit the Receiver App...");
            Console.ReadLine();
        }
    }
}