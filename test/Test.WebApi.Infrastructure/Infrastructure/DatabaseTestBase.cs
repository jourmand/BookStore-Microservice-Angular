using System;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.Commons;
using WebApi.Infrastructures.Data.Commons;

namespace Test.WebApi.Infrastructure.Infrastructure
{
    public class DatabaseTestBase : IDisposable
    {
        protected readonly BookStoreDbContext Context;

        public DatabaseTestBase()
        {
            var options = new DbContextOptionsBuilder<BookStoreDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var currentUserService = A.Fake<ICurrentUserService>();
            var domainEventHandlingExecutor = A.Fake<IDomainEventHandlingExecutor>();
            Context = new BookStoreDbContext(options, currentUserService, domainEventHandlingExecutor);

            Context.Database.EnsureCreated();

            DatabaseInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}
