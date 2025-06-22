using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "messages",
    durable: true,
    autoDelete: false,
    arguments: null,
    type: ExchangeType.Fanout
    );

await channel.QueueDeclareAsync(
    queue: "messageOne",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
    );

await channel.QueueBindAsync(queue: "messageOne", exchange: "messages", routingKey: string.Empty);

Console.WriteLine("Waiting for messages for messageOne...");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");

    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(
    queue: "messageOne",
    autoAck: false,
    consumer: consumer
);

Console.ReadLine(); // Keep the application running to listen for messages