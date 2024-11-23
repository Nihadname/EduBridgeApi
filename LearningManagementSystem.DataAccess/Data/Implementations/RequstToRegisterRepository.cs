using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public class RequstToRegisterRepository : Repository<RequestToRegister>, IRequstToRegisterRepository
    {
        public RequstToRegisterRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
