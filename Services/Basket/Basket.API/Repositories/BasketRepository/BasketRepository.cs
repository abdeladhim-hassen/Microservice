using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories.BasketRepository
{
    public class BasketRepository(IDistributedCache redistCache) : IBasketRepository
    {
        private readonly IDistributedCache _redistCache = redistCache ?? throw new ArgumentNullException(nameof(redistCache));

        public async Task Deletebasket(string username)
        {
            await _redistCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart?> GetBasket(string username)
        {
            var basket = await _redistCache.GetStringAsync(username);
            return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            if (string.IsNullOrEmpty(shoppingCart.UserName))
            {
                throw new InvalidOperationException("UserName cannot be null or empty.");
            }


            await _redistCache.SetStringAsync(
                shoppingCart.UserName,
                JsonConvert.SerializeObject(shoppingCart)
            );

            var updatedBasket = await GetBasket(shoppingCart.UserName);
            if (updatedBasket?.UserName == null)
            {
                throw new InvalidOperationException("Failed to update shopping cart.");
            }

            return updatedBasket;
        }
    }
}
