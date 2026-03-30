using ExpensesManager.Application.Handlers;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Services;
using ExpensesManager.Infrastructure.Settings;
using ExpensesManager.Infrastructure.Repositories; 
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services
builder.Services.AddControllers();
// Database - PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, MockPasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();  
app.UseSwaggerUI();  

app.MapControllers();

app.Run();
