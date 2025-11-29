using LogisticService.Application.Commands;
using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.ObjectStorage;
using LogisticService.Application.Services;
using LogisticService.Domain.Models.Shipping.ShippingFactory;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;
using LogisticService.Domain.Observer;
using LogisticService.Infrastructure.Context;
using LogisticService.Infrastructure.External;
using LogisticService.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ShippingOptimization.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "local";
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddGrpcClient<ShippingOptimizerService.ShippingOptimizerServiceClient>(options =>
    {
        options.Address = new Uri("http://localhost:7000"); 
    })
    .ConfigurePrimaryHttpMessageHandler(() => 
    {
        var handler = new HttpClientHandler();
        
        handler.ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    
        return handler;
    });

builder.Services.AddSingleton<ActiveShippingRegistry>();

builder.Services.AddTransient<IShippingObserver, DatabaseObserver>();

builder.Services.AddScoped<IObserverManager, ObserverManager>();

builder.Services.AddScoped<IShippingsRepository, ShippingsRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddScoped<IShippingFactory, AirShippingFactory>();
builder.Services.AddScoped<IShippingFactory, TruckShippingFactory>();
builder.Services.AddScoped<IShippingFactory, TrainShippingFactory>();
builder.Services.AddScoped<IShippingFactory, SeaShippingFactory>();


builder.Services.AddScoped<IVehicleProvider, VehicleProvider>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddScoped<IShippingOptimizer, GrpcShippingOptimizer>();
    //builder.Services.AddScoped<ShippingOptimizer>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<VehicleService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();