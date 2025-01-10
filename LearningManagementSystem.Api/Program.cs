using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hangfire;
using LearningManagementSystem.Api;
using LearningManagementSystem.Api.Middlewares;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Implementations;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Profiles;
using LearningManagementSystem.Application.Settings;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data;
using LearningManagementSystem.DataAccess.SeedDatas;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var config=builder.Configuration;
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IMapper>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

    using var scope = scopeFactory.CreateScope();
    var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
    //var photoOrVideoService = scope.ServiceProvider.GetRequiredService<IPhotoOrVideoService>();

    var mapperConfig = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MapperProfile(httpContextAccessor));
    });

    return mapperConfig.CreateMapper();
});

builder.Services.AddSingleton(provider =>
{
    var settings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;

    Console.WriteLine($"Initializing Cloudinary with: {settings.CloudName}, {settings.ApiKey}, {settings.ApiSecret}");

    var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
    var cloudinary = new Cloudinary(account);
    try
    {
        var result = cloudinary.ListResources(new ListResourcesParams { MaxResults = 1 });
        if (result.Error != null)
        {
            throw new CustomException(400,$"Cloudinary Account Error: {result.Error.Message}");
        }
    }
    catch (Exception ex)
    {
        throw new CustomException(400, ex.Message);
    }

    return cloudinary;
});

builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.Register(config);
builder.Services.AddHostedService<FeeProcessing>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHangfireDashboard();

app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.MigrateAsync();
    await UserSeed.SeedAdminUserAsync(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "error during migration");
};
app.Run();
