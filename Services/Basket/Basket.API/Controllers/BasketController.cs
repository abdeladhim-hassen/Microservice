using Basket.API.GrpcServices;
using Basket.API.Models;
using Basket.API.Repositories.BasketRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService) : ControllerBase
    {
        private readonly IBasketRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly DiscountGrpcService _DiscountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _DiscountGrpcService.GetDiscountAsync(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.Deletebasket(userName);
            return Ok();
        }
    }
}
