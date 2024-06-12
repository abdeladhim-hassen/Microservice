using Application.Contracts.Persistence;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository(DataContext dbContext) : RepositoryBase<Order>(dbContext), IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await _dbContext.Orders
                                   .Where(o => o.UserName == userName)
                                   .ToListAsync();
        }
    }

}
