using System.Threading;
using System.Threading.Tasks;
using WebApi.Core.Domain.Commons;

namespace WebApi.Infrastructures.Data.Commons
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookStoreDbContext _dbContext;

        public UnitOfWork(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
