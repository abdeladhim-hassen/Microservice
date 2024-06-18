using Basket.API.GrpcServices;
using Basket.API.Models;
using Basket.API.Repositories.BasketRepository;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController(IBasketRepository repository,
        DiscountGrpcService discountGrpcService,
        IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IBasketRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly DiscountGrpcService _DiscountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
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

        [HttpPost("Basket-Checkout", Name = "BasketCheckout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // Create and populate the BasketCheckoutEvent
            var eventMessage = new BasketCheckoutEvent
            {
                UserName = basketCheckout.UserName,
                TotalPrice = basket.TotalPrice,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                EmailAddress = basketCheckout.EmailAddress,
                AddressLine = basketCheckout.AddressLine,
                Country = basketCheckout.Country,
                State = basketCheckout.State,
                ZipCode = basketCheckout.ZipCode,
                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Expiration = basketCheckout.Expiration,
                CVV = basketCheckout.CVV,
                PaymentMethod = basketCheckout.PaymentMethod
            };

            // Send checkout event to RabbitMQ
            await _publishEndpoint.Publish(eventMessage);

            // Check if the basket.UserName is not null or empty before removing the basket
            if (!string.IsNullOrEmpty(basket.UserName))
            {
                await _repository.Deletebasket(basket.UserName);
            }
            else
            {
                return BadRequest("Invalid basket username.");
            }

            return Accepted();
        }


    }
}
