using LearningManagementSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class FeeProcessing : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<FeeProcessing> _logger;

    public FeeProcessing(IServiceProvider services, ILogger<FeeProcessing> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fee Processing Service Started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (now.Day == 1)
                {
                    _logger.LogInformation("Triggering Fee Creation for All Students.");
                    await CreateFeeToAllStudents(stoppingToken);
                }

                DateTime nextMonth = now.AddMonths(1);
                DateTime nextExecution = new DateTime(nextMonth.Year, nextMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan delayTime = nextExecution - now;

                _logger.LogInformation($"Next fee processing scheduled at {nextExecution} UTC.");

                await Task.Delay(delayTime, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Fee Processing Service stopped before next execution.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Fee Processing Service.");
            }
        }

        _logger.LogInformation("Fee Processing Service Stopped.");
    }

    private async Task CreateFeeToAllStudents(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Fee Creation Process.");

        using (var scope = _services.CreateScope())
        {
            var feeService = scope.ServiceProvider.GetRequiredService<IFeeService>();

            try
            {
                await feeService.CreateFeeToAllStudents(stoppingToken);
                _logger.LogInformation("Fee creation completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating fees.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Fee Processing Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
