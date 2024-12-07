using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public class ReportOptionRepository : Repository<ReportOption>, IReportOptionRepository
    {
        public ReportOptionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
