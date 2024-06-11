using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Features.Orders.Commands.ChekOutOrder;
using Application.Models;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler(IOrderRepository orderRepository,
        IEmailService emailService,
        ILogger<CheckoutOrderCommandHandler> logger)
        : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository = orderRepository 
            ?? throw new ArgumentNullException(nameof(orderRepository));

        private readonly IEmailService _emailService = emailService 
            ?? throw new ArgumentNullException(nameof(emailService));

        private readonly ILogger<CheckoutOrderCommandHandler> _logger = logger 
            ?? throw new ArgumentNullException(nameof(logger));

        public async Task<int> Handle(CheckoutOrderCommand request, 
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var orderEntity = MapToOrder(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation("Order {OrderId} was successfully created.", newOrder.Id);

            await SendEmailAsync(newOrder);

            return newOrder.Id;
        }

        private static Order MapToOrder(CheckoutOrderCommand command)
        {
            return new Order
            {
                UserName = command.UserName,
                TotalPrice = command.TotalPrice,
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress,
                AddressLine = command.AddressLine,
                Country = command.Country,
                State = command.State,
                ZipCode = command.ZipCode,
                CardName = command.CardName,
                CardNumber = command.CardNumber,
                Expiration = command.Expiration,
                CVV = command.CVV,
                PaymentMethod = command.PaymentMethod
            };
        }

        private async Task SendEmailAsync(Order order)
        {
            var email = new Email
            {
                To = "ezozkme@gmail.com",
                Body = "Order was created.",
                Subject = "Order Created"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order {OrderId} failed due to an error with the email service.", order.Id);
            }
        }
    }
}
