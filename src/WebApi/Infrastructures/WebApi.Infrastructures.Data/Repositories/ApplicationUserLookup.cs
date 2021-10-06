using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Repositories
{
    public class ApplicationUserLookup : IApplicationUserLookup
    {
        private DbSet<ApplicationUserInfo> DbSet { get; }
        public ApplicationUserLookup(BookStoreDbContext dbContext)
        {
            DbSet = dbContext.Set<ApplicationUserInfo>();
        }

        public async Task<ApplicationUserInfo> FindByEmail(string email) => 
            await DbSet.FirstOrDefaultAsync(o => o.Email == email);

        public async Task<ApplicationUserInfo> FindById(string id) =>
            await DbSet.FirstOrDefaultAsync(o => o.Id == new Guid(id));
    }
}