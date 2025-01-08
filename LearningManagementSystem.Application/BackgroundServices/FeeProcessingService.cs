using LearningManagementSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.BackgroundServices
{
    public class FeeProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public FeeProcessingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (DateTime.Now.Day == 1)
            {
                using var scope = _serviceProvider.CreateScope();
                var feeService = scope.ServiceProvider.GetRequiredService<IFeeService>();
                await feeService.CreateFeeToAllStudents();
            }
            await Task.Delay(TimeSpan.FromDays(1),stoppingToken);
        }
    }
}
