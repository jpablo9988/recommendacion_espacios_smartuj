using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;

namespace api_netcore_recommendacion_espacios.Servicios
{
    public static class MqttPublisherService
    {
        private static IMqttClient _mqttClient;
        private static IMqttClientOptions _mqttClientOptions;

        public static async Task RunAsync()
        {
            try
            {
                var factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient();
                _mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer("tcp://10.43.102.29", port : 1883)
                    .WithCredentials("admin","1234")
                    .WithCleanSession()
                    .Build();
                _mqttClient.UseConnectedHandler(e =>
                {
                     Console.WriteLine("MQTT Connected");
         
                });
                await _mqttClient.ConnectAsync(_mqttClientOptions);
                var testMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("test/topic")
                    .WithPayload("This is a test.")
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();
                await _mqttClient.PublishAsync(testMessage);
                await _mqttClient.DisconnectAsync();   

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

        }


    }
}
