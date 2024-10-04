using RabbitMQ.Client;
using System.Text;

namespace Producer;
public class Sender
{ 
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection()) 
        using (var chanel = connection.CreateModel())
        {
            chanel.QueueDeclare("BasicTest", false, false, false, null);

            string message = "Getting Started with .NET RabbitMQ";
            var body = Encoding.UTF8.GetBytes(message);

            chanel.BasicPublish("", "BasicTest", null, body);

            Console.WriteLine($"Send message {message}...");
        }

        Console.WriteLine("Press [Ente] to exit the Sender App...");
        Console.ReadLine();
    }
}