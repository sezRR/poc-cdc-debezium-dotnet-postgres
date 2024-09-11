using Confluent.Kafka;

ConsumerConfig config = new()
{
    GroupId = "postgres-cdc-group",
    BootstrapServers = "localhost:29092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe("fulfillment.public.Products");

CancellationTokenSource source = new();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    source.Cancel();
};

Console.WriteLine("Consuming messages...");

while (true)
{
    try
    {
        var consumeResult = consumer.Consume(source.Token);
        var message = consumeResult.Message.Value;
        Console.WriteLine($"Topic Name: {consumeResult.TopicPartitionOffset}");
        Console.WriteLine($"Message: {message}");
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Error occured: {e.Error.Reason}");
    }
}
