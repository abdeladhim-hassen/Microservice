using Application;
using Basket.API.Extensions;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using Ordering.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.AddMassTransitWithRabbitMqTransport(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
});

var app = builder.Build();

app.MigrateDatabase<DataContext>((context, services) =>
{
    var logger = services.GetService<ILogger<DataContextSeed>>();
    if (logger != null)
    {
        DataContextSeed
            .SeedAsync(context, logger)
            .Wait();
    }
    else
    {
        throw new InvalidOperationException("Logger service is not available.");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
