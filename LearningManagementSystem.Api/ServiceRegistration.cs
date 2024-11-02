using LearningManagementSystem.Core.Repositories;
using LearningManagementSystem.DataAccess.Data;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Api
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"))
        );
          
            services.AddScoped<ICourseRepository,CourseRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
    }
}
