using LearningManagementSystem.Api.App.ClientSide;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class NoteControllerTests
{
    private readonly Mock<INoteService> _noteServiceMock;
    private readonly NoteController _controller;

    public NoteControllerTests()
    {
        _noteServiceMock = new Mock<INoteService>();
        _controller = new NoteController(_noteServiceMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnOkResult_WhenNoteIsCreated()
    {
        // Arrange
        var noteCreateDto = new NoteCreateDto { Title = "Test Note", Description = "Test Description" };
        var noteReturnDto = new NoteReturnDto { Title = "Test Note", Description = "Test Description" };

        _noteServiceMock
            .Setup(service => service.Create(noteCreateDto))
            .ReturnsAsync(noteReturnDto);

        // Act
        var result = await _controller.Create(noteCreateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<NoteReturnDto>(okResult.Value);
        Assert.Equal("Test Note", returnValue.Title);
        Assert.Equal("Test Description", returnValue.Description);

        _noteServiceMock.Verify(service => service.Create(noteCreateDto), Times.Once);
    }
}
