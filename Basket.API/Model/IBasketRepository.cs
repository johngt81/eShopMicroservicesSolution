using System.Threading.Tasks;

namespace Basket.API.Model
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetCustomerBasketByIdAsync(string id);
    }
}