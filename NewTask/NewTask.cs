using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//channel.QueueDeclare(
//    queue: "hola",
//    durable: true,
//    exclusive: false,
//    autoDelete: false,
//    arguments: null
//    );

channel.QueueDeclare(
    queue: "task_queue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
    );

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: "hola",
//    basicProperties: null,
//    body: body
//    );

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: "task_queue",
    basicProperties: properties,
    body: body
    );

Console.WriteLine($" [x] Enviado: {message}");

Console.WriteLine(" Presione [enter] para salir.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hola mundo!");
}