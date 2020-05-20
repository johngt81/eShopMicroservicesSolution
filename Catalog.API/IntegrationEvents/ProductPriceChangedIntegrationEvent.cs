using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.IntegrationEvents
{
    public class ProductPriceChangedIntegrationEvent:IntegrationEvent
    {
        private readonly int productId;
        private readonly decimal oldPrice;
        private readonly decimal newPrice;

        public ProductPriceChangedIntegrationEvent(int productId, decimal oldPrice, decimal newPrice)
        {
            this.productId = productId;
            this.oldPrice = oldPrice;
            this.newPrice = newPrice;
        }
    }
}
