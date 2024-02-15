using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "hola",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null
    );

Console.WriteLine(" [*] Esperando mensajes.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Recibido: {message}");
};

channel.BasicConsume(
    queue: "hola",
    autoAck: true,
    consumer: consumer
    );

Console.WriteLine(" Presione [enter] para salir.");
Console.ReadLine();