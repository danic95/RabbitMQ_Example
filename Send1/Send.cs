using System;
using System.Text;
using RabbitMQ.Client;

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

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: "hola",
    basicProperties: null,
    body: body
    );

Console.WriteLine($" [x] Enviado: {message}");

Console.WriteLine(" Presione [enter] para salir.");
Console.ReadLine();
