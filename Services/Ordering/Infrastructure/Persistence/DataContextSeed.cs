using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext orderContext,
            ILogger<DataContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded database associated with context {DbContextName}",
                    nameof(DataContext));
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return
            [
                new Order
                {
                    UserName = "swn",
                    FirstName = "Mehmet",
                    LastName = "Ozkaya",
                    EmailAddress = "ezozkme@gmail.com",
                    AddressLine = "Bahcelievler",
                    Country = "Turkey",
                    TotalPrice = 350
                }
            ];
        }

    }
}
