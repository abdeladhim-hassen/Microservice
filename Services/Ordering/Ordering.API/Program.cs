using Infrastructure;
using Application;
using Ordering.API.Extensions;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
