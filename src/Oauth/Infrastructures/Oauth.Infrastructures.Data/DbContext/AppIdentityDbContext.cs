using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oauth.Infrastructures.Data.Entities;

namespace Oauth.Infrastructures.Data.DbContext
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("ApplicationUser_Claims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("ApplicationUser_Logins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("ApplicationUser_Tokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("ApplicationUser_RoleClaims"));
            builder.Entity<IdentityRole>(entity => entity.ToTable("ApplicationUser_Roles"));
            builder.Entity<ApplicationUser>(entity => entity.ToTable("ApplicationUser_Users"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("ApplicationUser_UserRoles"));
        }
    }
}