using architectureProject.Data;
using architectureProject.Models;
using architectureProject.Models.ShippingFactory;
using architectureProject.Repository;
using architectureProject.Repository.Decorators;
using architectureProject.ServiceControllers;
using architectureProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "local";
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IShippingsRepository, ShippingsRepository>();
builder.Services.Decorate<IShippingsRepository, CachedShippingsRepository>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddScoped<IShippingFactory, AirShippingFactory>();
builder.Services.AddScoped<IShippingFactory, TrackShippingFactory>();
builder.Services.AddScoped<IShippingFactory, TrainShippingFactory>();
builder.Services.AddScoped<IShippingFactory, SeaShippingFactory>();

builder.Services.AddScoped<ShippingOptimizer>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<VehicleService>();

//builder.Services.AddTransient<CreateShippingWithVehicleCommand>();
//builder.Services.AddTransient<GetOptimalShippingCommand>();

builder.Services.AddScoped<CommandHandler>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Shipping API", 
        Version = "v1",
        Description = "API для расчета стоимости перевозок"
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseStaticFiles();

app.MapControllers();

app.Run();