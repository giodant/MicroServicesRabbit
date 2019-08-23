using MicroRabbit.Infra.Bus;
using MicroServicesRabbit.Domain.Core.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain bus
            services.AddTransient<IEventBus, RabbitMQBus>();
        }
    }
}
