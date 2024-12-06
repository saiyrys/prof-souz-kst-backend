using Confluent.Kafka;
using Events.API.HostedService;
using Events.Application.Services;
using Events.Infrastructure.CacheService;
using Events.Infrastructure.Data;
using Events.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Events.Infrastructure.Messaging.Consumer;
using Events.Infrastructure.Messaging.Producer;
using Events.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.ConfigureWarnings(warnings => warnings.Throw());
});
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();


builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localHost:9092"
    };

    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services.AddSingleton<EventProducer>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<EventCache>();

builder.Services.AddScoped<EventConsumer>();
builder.Services.AddScoped<DataReadyConsumer>();

builder.Services.AddSingleton<IHostedService>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    var cacheService = provider.GetRequiredService<EventCache>();
    var lifeTime = provider.GetRequiredService<IHostApplicationLifetime>();

    return new ConsumerHostedService(scopeFactory,lifeTime);
});

var app = builder.Build();

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
