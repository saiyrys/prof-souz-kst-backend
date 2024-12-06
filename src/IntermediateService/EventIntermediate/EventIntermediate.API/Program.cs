using Confluent.Kafka;
using EventIntermediate.API.Consumers;
using EventIntermediate.API.EventHostedService;
using EventIntermediate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.ConfigureWarnings(warnings => warnings.Throw());
});
builder.Services.AddScoped<EventConsumer>();

builder.Services.AddSingleton<IHostedService>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new EventConsumerHostedService(scopeFactory);
});

builder.Services.AddSingleton<IConsumer<string, string>>(provider =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localHost:9092"
    };

    return new ConsumerBuilder<string, string>(config).Build();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
