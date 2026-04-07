using ExpensesManager.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ExpensesManager.Infrastructure.Services;

public class JwtBlackListCleanupService(IServiceScopeFactory scopeFactory): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBlackListRepository>();
                await repo.RemoveExpiredAsync();
            }
            
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}