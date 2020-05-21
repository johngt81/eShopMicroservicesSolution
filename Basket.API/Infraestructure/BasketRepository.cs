using Basket.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Infraestructure
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext basketContext;

        public BasketRepository(BasketContext basketContext)
        {
            this.basketContext = basketContext;
        }

        public IEnumerable<string> GetBuyers()
        {
            return this.basketContext.CustomerBaskets.Select(x => x.BuyerId);
        }

        public async Task<CustomerBasket> GetCustomerBasketByIdAsync(string id)
        {
            return await basketContext.CustomerBaskets.FirstOrDefaultAsync(x => x.BuyerId == id);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            this.basketContext.CustomerBaskets.Update(customerBasket);
            await this.basketContext.SaveChangesAsync();
            return await GetCustomerBasketByIdAsync(customerBasket.BuyerId);
        }
    }
}
