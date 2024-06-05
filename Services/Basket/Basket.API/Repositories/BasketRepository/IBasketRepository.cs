using Basket.API.Models;

namespace Basket.API.Repositories.BasketRepository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasket(string username);
        Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);
        Task Deletebasket(string username);

    }
}
