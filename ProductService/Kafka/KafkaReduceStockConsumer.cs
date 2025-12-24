using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Commands.ReduceStock;
 


namespace ProductService.Kafka;

public class KafkaReduceStockConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaReduceStockConsumer> _logger;

    public KafkaReduceStockConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<KafkaReduceStockConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    // protected override Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     return Task.Run(() => ConsumeAsync("order_created_v2", stoppingToken));
    // }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => ConsumeAsync("order_created_v2", stoppingToken);

    private async Task ConsumeAsync(string topic, CancellationToken ct)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "product-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        while (!ct.IsCancellationRequested)
        {
            var result = consumer.Consume(ct);
            if (result?.Message == null) continue;

            try
            {
                var items = JsonSerializer.Deserialize<List<ProductOrderDto>>(
                    result.Message.Value)!;

                await using var scope = _scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                foreach (var i in items)
                {
                    await mediator.Send(new ReduceStockCommand
                    {
                        ProductId = i.ProductId,
                        Quantity = -i.Quantity
                    });
                }

                consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consume error");
            }
        }
    }
     class ProductOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

  