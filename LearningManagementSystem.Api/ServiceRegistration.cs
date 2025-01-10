using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using LearningManagementSystem.Application.Implementations;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Profiles;
using LearningManagementSystem.Application.Settings;
using LearningManagementSystem.Application.Validators.AuthValidators;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Repositories;
using LearningManagementSystem.DataAccess.Data;
using LearningManagementSystem.DataAccess.Data.Implementations;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ZiggyCreatures.Caching.Fusion;

namespace LearningManagementSystem.Api
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"))
        );
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("AppConnectionString")));  
            services.AddHangfireServer();
            services.AddControllersWithViews()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value.Errors.First().ErrorMessage
                        );

                    var response = new
                    {
                        message = "Validation errors occurred.",
                        errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
          
            services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<RegisterValidator>();
            
            services.AddFluentValidationRulesToSwagger();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartEccommerceApi",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["Jwt:Issuer"],
               ValidAudience = configuration["Jwt:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
               ClockSkew = TimeSpan.Zero
           };
       });

         

            services.AddRepositories( AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.FullName.Contains("LearningManagementSystem.Core") || a.FullName.Contains("LearningManagementSystem.DataAccess"))
    .ToList());

            services.RegisterServices();
           
            services.AddMemoryCache();

            services.AddFusionCache()
                .WithDefaultEntryOptions(new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(2),
                    Priority = CacheItemPriority.High,

                });
            
        }
        public static void AddRepositories(this IServiceCollection services, List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var repositoryTypes = assembly.GetTypes()
                    .Where(type => !type.IsAbstract && !type.IsInterface
                        && type.GetInterfaces().Any(@interface => @interface.IsGenericType
                            && @interface.GetGenericTypeDefinition() == typeof(IRepository<>)))  
                    .ToList();

                foreach (var repositoryType in repositoryTypes)
                {
                    var interfaces = repositoryType.GetInterfaces()
                        .Where(@interface => !@interface.IsGenericType)  
                        .ToList();

                    if (interfaces.Any())
                    {
                        var interfaceToRegister = interfaces.FirstOrDefault();  
                        services.AddScoped(interfaceToRegister, repositoryType);
                    }
                }
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.Load("LearningManagementSystem.Application");

            var serviceTypes = applicationAssembly.GetTypes()
     .Where(t => t.IsClass && !t.IsAbstract)  
     .Where(t => !t.Name.Contains("Repository", StringComparison.OrdinalIgnoreCase))  
     .Where(t => t.Name.ToLower().Contains("service")) 
     .Where(t => t.IsPublic)  
     .Where(t => t.GetInterfaces().Any()) 
     .ToList();  
            foreach (var type in serviceTypes)
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    services.AddScoped(interfaceType, type);  
                }
            }

        }
    }
}
