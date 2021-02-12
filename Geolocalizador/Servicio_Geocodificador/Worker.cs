using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Mensajes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Geocodificador
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueRequest = "GeoResquest";
        private const string QueueResponse = "GeoResponse";
        private readonly AmqpInfo amqpInfo;

        public Worker(ILogger<Worker> logger, AmqpInfo options)
        {
            _logger = logger;

            this.amqpInfo = options;

            _connectionFactory = new ConnectionFactory
            {
                UserName = amqpInfo.Username,
                Password = amqpInfo.Password,
                VirtualHost = amqpInfo.VirtualHost,
                HostName = amqpInfo.HostName,
                Uri = new Uri(amqpInfo.Uri)
            };
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            //_channel.QueueDeclarePassive(QueueRequest);
            _channel.QueueDeclare(
              queue: QueueRequest,
              durable: false,
              exclusive: false,
              autoDelete: false,
              arguments: null
            );
            _channel.BasicQos(0, 1, false);
            _logger.LogInformation($"Queue [{QueueRequest}] is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    var geolocalizacion = JsonSerializer.Deserialize<PeticionGeolocalizacion>(message);
                    _logger.LogInformation($"Sending Geolocalización Request id: #{geolocalizacion.Id} .");

                    //3party api SourceStreetMap - Obtengo latitud y longitud, de la geoCodificacion

                    //Publico dicho resultado como PeticionGeolocalizacion en QueueResponse, para obtener dichos datos por parte de la api y persistir dicha información.

                    await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken); // simulate an async email process

                    _logger.LogInformation($"Geocodificacion Request id: #{geolocalizacion.Id}.");
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonException)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                }
                catch (Exception e)
                {
                    _logger.LogError(default, e, e.Message);
                }
            };

            _channel.BasicConsume(queue: QueueRequest, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
            _logger.LogInformation("RabbitMQ connection is closed.");
        }

    }
}
