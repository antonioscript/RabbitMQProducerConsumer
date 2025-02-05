using Commons;
using RabbitMQ.Client;
using System;
using System.Text;

class Sender
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

        // Criando o Exchange e a Fila
        channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
        channel.QueueDeclare(queue, false, false, false, null);

        // Ligando a Fila ao Exchange
        channel.QueueBind(queue, exchange, routingKey);

        // Enviando a mensagem
        string message = "Getting Started with .NET RabbitMQ";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange, routingKey, null, body);

        Console.WriteLine($"Send message: {message}...");
        Console.ReadLine();
    }
}
