using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IApplicationUserLookup _applicationUserLookup;
        private DbSet<ApplicationUserInfo> DbSet { get; }
        public ApplicationUserRepository(BookStoreDbContext dbContext,
            IApplicationUserLookup applicationUserLookup)
        {
            _applicationUserLookup = applicationUserLookup;
            DbSet = dbContext.Set<ApplicationUserInfo>();
        }

        public async Task<ApplicationUserInfo> Register(Events.UserCreatedIntegrationEvent user, CancellationToken cancellationToken = default)
        {
            var userInfo = ApplicationUserInfo.Create(Guid.NewGuid(), FullName.Create(user.FirstName, user.LastName),
                Email.Create(user.Email, _applicationUserLookup), Password.Create(user.Password));
            await DbSet.AddAsync(userInfo, cancellationToken);
            return userInfo;
        }

        public async Task<ApplicationUserInfo> FindByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(o => o.Email == email, cancellationToken: cancellationToken);
        }
    }
}