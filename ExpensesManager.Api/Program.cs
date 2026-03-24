using ExpensesManager.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

app.MapControllers();

app.Run();
