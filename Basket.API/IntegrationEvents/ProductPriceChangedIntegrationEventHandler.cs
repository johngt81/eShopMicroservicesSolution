using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        public Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
