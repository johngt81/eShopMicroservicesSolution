﻿using System;

namespace EventBus
{
    public class IntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
