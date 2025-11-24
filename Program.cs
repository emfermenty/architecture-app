using gprcoptimizer.Models.Shipping.ShippingFactory.Interfaces;
using gprcoptimizer.Models.Shipping.ShippingFactory;
using gprcoptimizer.Services.Interfaces;
using gprcoptimizer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });

    options.ListenAnyIP(5000);
});

builder.Services.AddSingleton<IShippingOptimizer, ShippingOptimizer>();
builder.Services.AddSingleton<IShippingFactory, TruckShippingFactory>();
builder.Services.AddSingleton<IShippingFactory, SeaShippingFactory>();
builder.Services.AddSingleton<IShippingFactory, TrainShippingFactory>();
builder.Services.AddSingleton<IShippingFactory, AirShippingFactory>();

var app = builder.Build();

app.MapGrpcService<ShippingOptimizerGrpcService>();
app.MapGet("/", () => "грпц работает");

app.Run();