using EventBus;

namespace Catalog.API.IntegrationEvents
{
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
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
