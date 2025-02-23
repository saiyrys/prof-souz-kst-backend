using Auth.Application.Constants;
using Auth.Application.Interface;
using Auth.Application.Services;
using Auth.Domain.Data;
using Auth.Domain.Interface;
using Auth.Infrastructure.Factories;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.SomeService.GenerationPassword;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.ConfigureWarnings(warnings => warnings.Throw());
    
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IControl<>), typeof(Control<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<UserFactory, CreateUserFactory>();
builder.Services.AddScoped<TokenGeneration>();
builder.Services.AddScoped<IHashingPassword, HashingPassword>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();



AuthConfig.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }
