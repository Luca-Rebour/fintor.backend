using Application.Interfaces.UseCases.RecurringTransactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class RecurringTransactionHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public RecurringTransactionHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var generator = scope.ServiceProvider.GetRequiredService<IGenerateRecurringTransactions>();
            await generator.ExecuteAsync();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}

