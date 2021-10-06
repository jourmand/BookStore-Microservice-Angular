using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;

namespace WebApi.Infrastructures.Data.Configuration
{
    public class ApplicationUserInfoConfig : IEntityTypeConfiguration<ApplicationUserInfo>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserInfo> builder)
        {
            builder.ToTable("ApplicationUsers");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .ValueGeneratedNever();

            builder.OwnsOne(c => c.FullName, d =>
            {
                d.Property(e => e.FirstName).IsUnicode().HasMaxLength(150).IsRequired().HasColumnName("FirstName");
                d.Property(e => e.LastName).IsUnicode().HasMaxLength(150).IsRequired().HasColumnName("LastName");
            })
                .HasData(new
                {
                    Id = new Guid("8b65574d-8394-4574-8e3b-f1a9b76e50c9"),
                    Email = new Email("admin@live.com"),
                    FirstName = "Rasoul",
                    LastName = "Jourmand",
                    Created = new DateTime(2022, 10, 12, 11, 22, 14)
                });

            builder.Property(c => c.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasConversion(c => c.Value, d => new Email(d));
        }
    }
}
