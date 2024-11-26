using LearningManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public interface IUnitOfWork
    {
        public ICourseRepository CourseRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ITeacherRepository TeacherRepository { get; }
        public IParentRepository  ParentRepository { get; }
        public Task Commit();
       public Task<IDbContextTransaction> BeginTransactionAsync();
        public IRequstToRegisterRepository RequstToRegisterRepository { get; }
        public ICourseRepository courseRepository   { get; }
    }
}
