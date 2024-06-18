using Application.Features.Orders.Commands.ChekOutOrder;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger) : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = new CheckoutOrderCommand
            {
                UserName = context.Message.UserName,
                TotalPrice = context.Message.TotalPrice,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,
                EmailAddress = context.Message.EmailAddress,
                AddressLine = context.Message.AddressLine,
                Country = context.Message.Country,
                State = context.Message.State,
                ZipCode = context.Message.ZipCode,
                CardName = context.Message.CardName,
                CardNumber = context.Message.CardNumber,
                Expiration = context.Message.Expiration,
                CVV = context.Message.CVV,
                PaymentMethod = context.Message.PaymentMethod
            };

            var result = await _mediator.Send(command);

            _logger.LogInformation("Consumed {EventName} successfully. Created Order Id: {OrderId}", nameof(BasketCheckoutEvent), result);
        }
    }
}
