using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public interface IIntegrationEventHandler<T>
        where T:IntegrationEvent
    {
    }
}
