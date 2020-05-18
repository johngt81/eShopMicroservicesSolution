using Basket.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<CustomerBasket> GetCustomerBasketByIdAsync(string id)
        {
            return await basketContext.CustomerBaskets.FirstOrDefaultAsync(x => x.BuyerId == id);
        }
    }
}
