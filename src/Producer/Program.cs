using RabbitMQ.Client;

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

for (int i = 0; i < 5; i++)
{
    var message = $"Message {DateTime.UtcNow} {i + 1}";
    var body = System.Text.Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(
        exchange: "messages",
        routingKey: string.Empty,
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body
    );

    await Task.Delay(1000); // Simulate some delay between messages
}
