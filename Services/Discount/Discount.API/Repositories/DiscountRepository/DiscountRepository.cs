﻿using Dapper;
using Discount.API.Models;
using Npgsql;

namespace Discount.API.Repositories.DiscountRepository
{
    public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
    {
        private readonly string _connectionString = configuration?
            .GetConnectionString("Postgres")
                ?? throw new ArgumentNullException(nameof(configuration));

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", 
                new {
                         ProductName = productName
                    });

            return coupon ?? new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { 
                        coupon.ProductName,
                        coupon.Description,
                        coupon.Amount 
                    });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
                new { 
                        coupon.ProductName, 
                        coupon.Description,
                        coupon.Amount,
                        coupon.Id 
                    });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new {
                        ProductName = productName 
                    });

            return affected > 0;
        }
    }
}
