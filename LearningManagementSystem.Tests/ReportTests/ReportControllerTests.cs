using LearningManagementSystem.Api.App.ClientSide;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Tests.ReportTests
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportService> _ReportServiceMock;
        private readonly ReportController reportController;
        public ReportControllerTests()
        {
            _ReportServiceMock = new Mock<IReportService>();
            reportController = new ReportController(_ReportServiceMock.Object);
        }
        [Fact]
        public async Task Create_ShouldReturnOkResult_WhenNoteIsCreated()
        {
            var generatedGuid=Guid.NewGuid();
            var reportCreateDto = new ReportCreateDto { Description = "wqqdwqdwd2424", ReportedUserId = "efwfwefwef", ReportOptionId = generatedGuid };
            var reportReturnDto = new ReportReturnDto { Description = "wqqdwqdwd2424", userReportReturnDto = null, optionInReportReturnDto = null };
            _ReportServiceMock
            .Setup(service => service.Create(reportCreateDto))
            .ReturnsAsync(reportReturnDto);
            var result = await reportController.Create(reportCreateDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReportReturnDto>(okResult.Value);
            Assert.Equal("wqqdwqdwd2424", returnValue.Description);

            _ReportServiceMock.Verify(service => service.Create(reportCreateDto), Times.Once);

        }
    }
}
