using Application.Contracts.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            return orderList.Select(MapToOrdersVm).ToList();
        }

        private OrdersVm MapToOrdersVm(Order order)
        {
            return new OrdersVm
            {
                Id = order.Id,
                UserName = order.UserName,
                TotalPrice = order.TotalPrice,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                AddressLine = order.AddressLine,
                Country = order.Country,
                State = order.State,
                ZipCode = order.ZipCode,
                CardName = order.CardName,
                CardNumber = order.CardNumber,
                Expiration = order.Expiration,
                CVV = order.CVV,
                PaymentMethod = order.PaymentMethod
            };
        }
    }
}
