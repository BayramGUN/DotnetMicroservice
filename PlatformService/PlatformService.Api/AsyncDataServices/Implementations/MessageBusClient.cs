using System.Text;
using System.Text.Json;
using PlatformService.Api.AsyncDataServices.Interfaces;
using PlatformService.Api.DTOs;
using RabbitMQ.Client;

namespace PlatformService.Api.AsyncDataServices.Implementations;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection = null!;
    private readonly IModel _channel = null!;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory() {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]!)
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
            Console.WriteLine("--> Connected to Message Bus!");
        }
        catch(Exception exception)
        {
            Console.WriteLine($"--> Could not connect to Message Bus: {exception.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if(_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ connection Open, sending message...");
            SendMessage(message);
        }
        else
            Console.WriteLine("--> RabbitMQ connection is closed, not sending message...");
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "trigger",
                              routingKey: "",
                              basicProperties: null,
                              body: body);
        Console.WriteLine($"We have sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("--> MessageBus Disposed");
        if(_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
    {
        Console.WriteLine($"--> RabbitMQ Connection shuted Down");
    }
}