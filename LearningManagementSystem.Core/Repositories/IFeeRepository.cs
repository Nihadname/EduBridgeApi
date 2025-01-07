using LearningManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Repositories
{
    public interface IFeeRepository:IRepository<Fee>
    {
        Task<Fee> GetLaastFeeAsync(Expression<Func<Fee, bool>> predicate);
    }
}
