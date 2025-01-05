using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public class FeeRepository : Repository<Fee>, IFeeRepository
    {
        public FeeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
