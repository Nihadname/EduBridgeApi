using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Implementations
{
    public class FeeRepository : Repository<Fee>, IFeeRepository
    {
        protected readonly ApplicationDbContext _context;
        public FeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Fee> GetLaastFeeAsync(Expression<Func<Fee, bool>> predicate)
        {
            return await _context.fees.OrderByDescending(f => f.DueDate).FirstOrDefaultAsync(predicate);
        }
    }
}
