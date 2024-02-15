using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

//garantizar que por cada worker solamente se envie un mensaje, por lo que mientras un worker no confirme la recepcion de un mensaje, este no reciba mas mensajes.
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Esperando mensajes.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Recibido: {message}");

    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 1000);

    Console.WriteLine(" [x] Terminado");

    //aqui 'channel' podria ser accesado tambien como ((EventingBasicConsumer)sender).Model
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

//channel.BasicConsume(
//    queue: "hola",
//    autoAck: false,
//    consumer: consumer
//    );

channel.BasicConsume(
    queue: "task_queue",
    autoAck: false,
    consumer: consumer
    );

Console.WriteLine(" Presione [enter] para salir.");
Console.ReadLine();