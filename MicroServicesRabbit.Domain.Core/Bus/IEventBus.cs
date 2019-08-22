using MicroServicesRabbit.Domain.Core.Commands;
using MicroServicesRabbit.Domain.Core.Events;
using System.Threading.Tasks;

namespace MicroServicesRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;
        void Publish<T>(T @event) where T : Event;
        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler;
    }
}
