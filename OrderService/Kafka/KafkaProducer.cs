using Confluent.Kafka;
namespace OrderService.Kafka;
public interface IKafkaProducer
{
    Task ProduceAsync(string topic, Message<string,string> message);
}


public class KafkaProducer : IKafkaProducer
{

    private readonly IProducer<string,string> _producer;
    public KafkaProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092" // địa chỉ của Kafka broker
            // Các cấu hình khác nếu cần 
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public Task ProduceAsync(string topic, Message<string, string> message)
    {
        return _producer.ProduceAsync(topic, message);
    }
}