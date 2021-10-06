using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Core.Domain.BookAggregate.Contracts
{
    public interface IBookItemRepository
    {
        Task<BookItem> AddItem(BookItem bookItem, CancellationToken cancellationToken = default);

        Task<BookItem> FindById(Guid id, CancellationToken cancellationToken = default);
    }
}
