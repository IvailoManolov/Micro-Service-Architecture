﻿using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WebApplication1.DTOs;

namespace WebApplication1.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() 
            { 
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"--> Message bus problem: {ex.Message}");
            }    
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection shutdown!");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublished)
        {
            var message = JsonSerializer.Serialize(platformPublished);

            if (_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ Connection Open, sending message : {message}");

                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is Closed!");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                    routingKey: "",
                    basicProperties: null,
                    body: body);

            Console.WriteLine($"--> Message sent: {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBusClient disposed!");

            if(_channel.IsOpen) 
            { 
                _channel.Close();
                _connection.Close();
            }
        }
    }
}