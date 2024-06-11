
using MediatR;

namespace Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery(string userName) : IRequest<List<OrdersVm>>
    {
        public string UserName { get; set; } = userName;
    }
}
