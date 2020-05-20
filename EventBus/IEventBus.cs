﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<T,TH>()
            where T:IntegrationEvent
            where TH:IIntegrationEventHandler<T>;
    }
}