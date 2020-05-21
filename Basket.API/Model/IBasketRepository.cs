using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Model
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetCustomerBasketByIdAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket);
        IEnumerable<string> GetBuyers();
    }
}