using Application.Interfaces.UseCases.RecurringTransactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class RecurringTransactionHostedService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<RecurringTransactionHostedService> _logger;
	private readonly IHostEnvironment _env;

	public RecurringTransactionHostedService(
		IServiceProvider serviceProvider,
		ILogger<RecurringTransactionHostedService> logger,
		IHostEnvironment env)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
		_env = env;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("RecurringTransactionHostedService started at {UtcNow}", DateTime.UtcNow);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				using var scope = _serviceProvider.CreateScope();
				var generator = scope.ServiceProvider.GetRequiredService<IGenerateRecurringTransactions>();

				_logger.LogInformation("Generating recurring transactions at {UtcNow}", DateTime.UtcNow);
				await generator.ExecuteAsync();
				_logger.LogInformation("Done generating recurring transactions at {UtcNow}", DateTime.UtcNow);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error running recurring transaction job");
			}

			// En dev: corré una vez y salí (ideal para test)
			if (_env.IsDevelopment())
				break;

			await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
		}

		_logger.LogInformation("RecurringTransactionHostedService stopped at {UtcNow}", DateTime.UtcNow);
	}
}