using MicroServicesRabbit.Domain.Core.Events;
using System;

namespace MicroServicesRabbit.Domain.Core.Commands
{
    public abstract class Command: Message
    {
        public DateTime PublishDate { get; protected set; }

        protected Command()
        {
            PublishDate = DateTime.Now;
        }
    }
}
