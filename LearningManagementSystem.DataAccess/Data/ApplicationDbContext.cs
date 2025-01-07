using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Parent> parents { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Lesson> lessons { get; set; }
        public DbSet<LessonStudent> lessonsStudents { get; set; }
        public DbSet<LessonMaterial> lessonsMaterial { get; set; }
        public DbSet<LessonQuiz> lessonQuizzes { get; set; }
        public DbSet<LessonVideo> lessonsVideo { get; set; }
        public DbSet<Note> notes { get; set; }
        public DbSet<QuizOption> quizOptions { get; set; }
        public DbSet<QuizQuestion > quizQuestions { get; set; }
        public DbSet<QuizResult> quizResults { get; set; }
        public DbSet<RequestToRegister> requestToRegister { get; set; }
        public DbSet<Fee> fees { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<ReportOption> reportOptions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
