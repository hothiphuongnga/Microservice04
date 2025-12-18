using Confluent.Kafka;
using System.Text.Json;
using ProductService.Services;

namespace ProductService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(IServiceProvider serviceProvider, ILogger<KafkaConsumer> logger)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "product-service-group", // tạo group id riêng cho product service để tránh xung đột với các service khác
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("order_created"); // subscribe topic 'order_created'
            _logger.LogInformation("Kafka Consumer đã subscribe topic 'order_created'"); 

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);
                        
                        if (consumeResult != null)
                        {
                            _logger.LogInformation($"Nhận được message từ Kafka: Key={consumeResult.Message.Key}");
                            
                            // Deserialize message
                            var orderDetails = JsonSerializer.Deserialize<List<ProductOrderDto>>(consumeResult.Message.Value);
                            
                            if (orderDetails != null && orderDetails.Any())
                            {
                                // Xử lý cập nhật tồn kho
                                await UpdateProductStock(orderDetails);
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Lỗi khi consume message: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Lỗi khi xử lý message: {ex.Message}");
                    }

                    await Task.Delay(100, stoppingToken);
                }
            }
            finally
            {
                _consumer.Close();
                _logger.LogInformation("Kafka Consumer đã đóng");
            }
        }

        private async Task UpdateProductStock(List<ProductOrderDto> orderDetails)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                
                foreach (var item in orderDetails)
                {
                    try
                    {
                        _logger.LogInformation($"Cập nhật tồn kho cho ProductId={item.ProductId}, Quantity={-item.Quantity}");
                        
                        // Gọi UpdateStockAsync với số lượng âm để giảm tồn kho
                        var result = await productService.UpdateStockAsync(item.ProductId, -item.Quantity);
                        
                        if (result.StatusCode == 200)
                        {
                            _logger.LogInformation($"Cập nhật tồn kho thành công cho ProductId={item.ProductId}");
                        }
                        else
                        {
                            _logger.LogWarning($"Cập nhật tồn kho thất bại cho ProductId={item.ProductId}: {result.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Lỗi khi cập nhật tồn kho cho ProductId={item.ProductId}: {ex.Message}");
                    }
                }
            }
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }

    // DTO để deserialize message từ Kafka
    public class ProductOrderDto
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
