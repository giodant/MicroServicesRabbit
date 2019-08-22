using MicroServicesRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

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
