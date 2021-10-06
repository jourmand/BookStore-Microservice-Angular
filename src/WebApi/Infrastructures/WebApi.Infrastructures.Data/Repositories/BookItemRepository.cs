using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Repositories
{
    public class BookItemRepository : IBookItemRepository
    {
        private DbSet<BookItem> DbSet { get; }
        public BookItemRepository(BookStoreDbContext dbContext)
        {
            DbSet = dbContext.Set<BookItem>();
        }

        public async Task<BookItem> AddItem(BookItem bookItem, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(bookItem, cancellationToken);
            return bookItem;
        }

        public async Task<BookItem> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(o => o.Subscriptions)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
    }
}