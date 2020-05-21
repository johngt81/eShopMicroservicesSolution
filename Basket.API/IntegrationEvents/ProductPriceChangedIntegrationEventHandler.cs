using Basket.API.Model;
using EventBus;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        private readonly IBasketRepository basketRepository;

        public ProductPriceChangedIntegrationEventHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            var userIds = this.basketRepository.GetBuyers();

            foreach (var id in userIds)
            {
                var basket = await this.basketRepository.GetCustomerBasketByIdAsync(id);
                var item = basket.Items.FirstOrDefault(x => x.ProductId == @event.ProductId);
                item.OldUnitPrice = @event.OldPrice;
                item.UnitPrice = @event.NewPrice;

                await this.basketRepository.UpdateBasketAsync(basket);
            }
        }
    }
}
