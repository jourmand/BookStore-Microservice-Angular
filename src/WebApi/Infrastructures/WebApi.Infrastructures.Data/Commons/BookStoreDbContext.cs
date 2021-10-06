using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.Commons;

namespace WebApi.Infrastructures.Data.Commons
{
    public class BookStoreDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDomainEventHandlingExecutor _domainEventHandlingExecutor;

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options,
            ICurrentUserService currentUserService,
            IDomainEventHandlingExecutor domainEventHandlingExecutor) : base(options)
        {
            _currentUserService = currentUserService;
            _domainEventHandlingExecutor = domainEventHandlingExecutor;
        }

        public DbSet<ApplicationUserInfo> ApplicationUserInfos { get; set; }
        public DbSet<BookItem> BookItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = DateTime.Now;
                        break;
                }
            }
            _domainEventHandlingExecutor.Execute(GetDomainEventEntities(ChangeTracker));
            var changeCount = base.SaveChangesAsync(cancellationToken);
            return changeCount;
        }

        private IEnumerable<AggregateRoot> GetDomainEventEntities(ChangeTracker changeTracker)
        {
            return changeTracker.Entries<AggregateRoot>()
                .Select(po => po.Entity)
                .Where(po => po.GetChanges().Any())
                .ToArray();
        }
    }
}
