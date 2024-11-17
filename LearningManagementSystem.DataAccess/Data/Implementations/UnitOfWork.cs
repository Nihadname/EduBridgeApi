using LearningManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public ICourseRepository CourseRepository { get; private set; }
        public IStudentRepository StudentRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            CourseRepository = new CourseRepository(applicationDbContext);
            StudentRepository = new StudentRepository(applicationDbContext);
            _applicationDbContext = applicationDbContext;

        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _applicationDbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
