using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Repositories
{
    public class BookItemLookup : IBookItemLookup
    {
        private DbSet<BookItem> DbSet { get; }
        public BookItemLookup(BookStoreDbContext dbContext)
        {
            DbSet = dbContext.Set<BookItem>();
        }

        public async Task<BookItem> FindByName(string name)
        {
            return await DbSet.FirstOrDefaultAsync(o => o.Name == name);
        }
    }
}