using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Configure Ocelot with caching
builder.Services.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });

var app = builder.Build();
app.UseOcelot().Wait();
app.Run();
