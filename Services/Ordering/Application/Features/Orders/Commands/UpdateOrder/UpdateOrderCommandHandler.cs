using Application.Contracts.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger) : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly ILogger<UpdateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id) ?? throw new Exception($"Order with ID {request.Id} was not found.");
            MapRequestToOrder(request, orderToUpdate);

            await _orderRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation("Order {OrderId} was successfully deleted.", orderToUpdate.Id);

            return Unit.Value;
        }

        private static void MapRequestToOrder(UpdateOrderCommand request, Order orderToUpdate)
        {
            orderToUpdate.UserName = request.UserName;
            orderToUpdate.TotalPrice = request.TotalPrice;
            orderToUpdate.FirstName = request.FirstName;
            orderToUpdate.LastName = request.LastName;
            orderToUpdate.EmailAddress = request.EmailAddress;
            orderToUpdate.AddressLine = request.AddressLine;
            orderToUpdate.Country = request.Country;
            orderToUpdate.State = request.State;
            orderToUpdate.ZipCode = request.ZipCode;
            orderToUpdate.CardName = request.CardName;
            orderToUpdate.CardNumber = request.CardNumber;
            orderToUpdate.Expiration = request.Expiration;
            orderToUpdate.CVV = request.CVV;
            orderToUpdate.PaymentMethod = request.PaymentMethod;
        }
    }
}
