using AutoMapper;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Implementations;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Tests.NoteTests
{
    public class NoteServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _MapperMock;
        private readonly Mock<UserManager<AppUser>> _UserManagerMock;
        private readonly Mock<IHttpContextAccessor> _HttpContextAccessorMock;
        private readonly NoteService _noteService;

        public NoteServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _MapperMock = new Mock<IMapper>();
            _UserManagerMock = CreateUserManagerMock();
            _HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
             _noteService = new NoteService(
         _MapperMock.Object,
         _unitOfWorkMock.Object,
         _UserManagerMock.Object,
         _HttpContextAccessorMock.Object);
        }
        [Fact]
        public async Task Create_ShouldReturnNoteReturnDto_WhenValidInput()
        {
            var userId = "random";
            var appUser=new AppUser() { Id=userId,Notes=new List<Note>()};
            var noteCreateDto = new NoteCreateDto { Title = "New Note", Description = "Description" };
            var note = new Note { Title = "New Note", Description = "Description", AppUserId = userId };
            var noteReturnDto = new NoteReturnDto { Title = "New Note", Description = "Description" };
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, userId)
};
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Mock IHttpContextAccessor to return the ClaimsPrincipal
            _HttpContextAccessorMock
                .Setup(h => h.HttpContext.User)
                .Returns(claimsPrincipal);
            _unitOfWorkMock
            .Setup(u => u.NoteRepository.Create(It.IsAny<Note>()))
            .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(s=>s.Commit()).Returns(Task.CompletedTask);
            _MapperMock
           .Setup(m => m.Map<Note>(It.IsAny<NoteCreateDto>()))
           .Returns(note);
            _MapperMock
         .Setup(m => m.Map<NoteReturnDto>(It.IsAny<Note>()))
         .Returns(noteReturnDto);
            var result = await _noteService.Create(noteCreateDto);
            Assert.NotNull(result);
            Assert.Equal("New Note", result.Title);
            Assert.Equal("Description", result.Description);
            _unitOfWorkMock.Verify(u => u.NoteRepository.Create(It.IsAny<Note>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            _MapperMock.Verify(m => m.Map<Note>(noteCreateDto), Times.Once);
            _MapperMock.Verify(m => m.Map<NoteReturnDto>(note), Times.Once);
        }
        private Mock<UserManager<AppUser>> CreateUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object,
                null, // IOptions<IdentityOptions>
                null, // IPasswordHasher<AppUser>
                null, // IEnumerable<IUserValidator<AppUser>>
                null, // IEnumerable<IPasswordValidator<AppUser>>
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null  // ILogger<UserManager<AppUser>>
            );
        }

    }
}
