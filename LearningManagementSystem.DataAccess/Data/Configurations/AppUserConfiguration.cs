using LearningManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataAccess.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(s => s.UserName).HasMaxLength(100).IsRequired(true);
            builder.Property(s => s.fullName).HasMaxLength(150).IsRequired(true);
            builder.Property(s => s.Email).IsRequired()
            .HasMaxLength(255)
            .HasAnnotation("RegularExpression",
                           @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            builder.Property(s => s.IsBlocked).HasDefaultValue(false);
            
        }
    }
}
