using LearningManagementSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace LearningManagementSystem.Tests.BackgroundTests
{
    public class FeeProcessingTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldCallCreateFeeToAllStudents()
        {
            // Arrange
            var mockFeeService = new Mock<IFeeService>();
            var mockLogger = new Mock<ILogger<FeeProcessing>>();
            var serviceProvider = new ServiceCollection()
                .AddScoped(_ => mockFeeService.Object)
                .BuildServiceProvider();

            var feeProcessing = new FeeProcessing(serviceProvider, mockLogger.Object);

            // Act
            await feeProcessing.StartAsync(CancellationToken.None);

            // Assert
            mockFeeService.Verify(
                service => service.CreateFeeToAllStudents(It.IsAny<CancellationToken>()),
                Times.Once);

            mockLogger.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Consume Scoped Service Hosted Service is working.")),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
