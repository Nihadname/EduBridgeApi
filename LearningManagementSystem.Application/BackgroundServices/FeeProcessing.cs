using LearningManagementSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class FeeProcessing : BackgroundService
{

    private readonly ILogger<FeeProcessing> _logger;

    public FeeProcessing(IServiceProvider services,
        ILogger<FeeProcessing> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        await DoWork(stoppingToken);
    }
    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        using (var scope = Services.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IFeeService>();

            await scopedProcessingService.CreateFeeToAllStudents(stoppingToken);
        }
    }
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}



