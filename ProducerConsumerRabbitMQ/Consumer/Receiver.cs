using Commons;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Producer;
public class Receiver
{
    public static void Main(string[] args)
    {
        //Configuration
        string exchange = RabbitMQConstants.Exchange;
        string routingKey = RabbitMQConstants.RoutingKey;
        string queue = RabbitMQConstants.Queue;
        string hostName = RabbitMQConstants.HostName;

        // Criando a conexão com o RabbitMQ
        var factory = new ConnectionFactory() { HostName = hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declarando o Exchange e a Fila
        channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
        channel.QueueDeclare(queue, false, false, false, null);

        // Ligando a Fila ao Exchange
        channel.QueueBind(queue, exchange, routingKey);

        // Consumindo a mensagem
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Mensagem recebida: {message}");
        };

        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

        Console.WriteLine("Aguardando mensagens...");
        Console.ReadLine();  // Manter o consumidor rodando
    }
}
