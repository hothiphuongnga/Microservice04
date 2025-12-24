using Confluent.Kafka;
using System.Text.Json;
using ProductService.Services;

namespace ProductService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync("order_created", stoppingToken), stoppingToken);
        }

        private async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "product-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,

                EnableAutoCommit = false // ❗ BẮT BUỘC
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            _logger.LogInformation("KafkaConsumer subscribed topic: {Topic}", topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult?.Message == null) continue;

                    try
                    {
                        var orderDetails = JsonSerializer.Deserialize<List<ProductOrderDto>>(
                            consumeResult.Message.Value
                        );

                        if (orderDetails == null || !orderDetails.Any())
                        {
                            _logger.LogWarning("Kafka message empty or invalid");
                            consumer.Commit(consumeResult);
                            continue;
                        }

                        using var scope = _scopeFactory.CreateScope();
                        var productService = scope.ServiceProvider
                            .GetRequiredService<IProductService>();

                        foreach (var item in orderDetails)
                        {
                            _logger.LogInformation(
                                "Update stock ProductId={ProductId}, Qty={Qty}",
                                item.ProductId,
                                -item.Quantity
                            );

                            var result = await productService
                                .UpdateStockAsync(item.ProductId, -item.Quantity);

                            if (result.StatusCode != 200)
                            {
                                throw new Exception(result.Message);
                            }
                        }

                        // ✅ COMMIT SAU KHI XỬ LÝ THÀNH CÔNG
                        consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing Kafka message");
                        // ❌ KHÔNG COMMIT → Kafka sẽ retry
                    }
                }
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("KafkaConsumer closed");
            }
        }
    }

    // DTO
    class ProductOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
