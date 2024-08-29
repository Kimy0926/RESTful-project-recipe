using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMSApiServer
{
    public class KafkaService
    {
        private readonly KafkaSettings _kafkaSettings;

        public KafkaService(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
        }

        public async Task ProduceMessageAsync(string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers
            };

            using IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();

            Message<string, string> sendMessage = new Message<string, string>
            {
                Value = message
            };

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                Task<DeliveryResult<string, string>> produceTask = producer.ProduceAsync(_kafkaSettings.ProducerTopic, sendMessage);

                DeliveryResult<string, string> deliveryResult = await Task.WhenAny(produceTask, Task.Delay(Timeout.Infinite, cts.Token)) == produceTask
                    ? await produceTask
                    : throw new TimeoutException("The Kafka produce operation timed out.");

                Console.WriteLine($"Delivered '{deliveryResult.Topic}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout occurred: {ex.Message}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Kafka produce error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }


        public class KafkaSettings
        {
            public string BootstrapServers { get; set; }
            public string ProducerTopic { get; set; }
            public string ConsumerTopic { get; set; }
            public string GroupId { get; set; }
        }
    }

}
