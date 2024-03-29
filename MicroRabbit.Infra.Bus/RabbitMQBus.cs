﻿using MediatR;
using MicroServicesRabbit.Domain.Core.Bus;
using MicroServicesRabbit.Domain.Core.Commands;
using MicroServicesRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator
                           , IServiceScopeFactory serviceProviderFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceProviderFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var eventName = @event.GetType().Name;
                    var queue = channel.QueueDeclare(eventName, false, false, false, null);
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    channel.BasicPublish("", eventName, null, body);
                }
            }
        }
   

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler
        {
            var eventType = typeof(T);
            string eventName = eventType.Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(eventType))
            {
                _eventTypes.Add(eventType);
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'"
                                            ,nameof(handlerType));
            }
            //Add event handler to the list of event 'eventName' handlers
            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            //Declare factory
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            //Create Connection And Communication Channel
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var eventName = typeof(T).Name;
            //Declare eventName Queue
            channel.QueueDeclare(eventName, false, false, false, null);
            //Add async consumer
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            //Basic consume with Auto Ack
            channel.BasicConsume(eventName, true, consumer);

        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body);
            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if(_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        //Get handler from Service Provider
                        var handler = scope.ServiceProvider.GetRequiredService(subscription);
                        if (handler == null)
                            continue;

                        var eventType = _eventTypes.SingleOrDefault(x => x.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concretType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concretType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
                    
            }
        }
    }
}
