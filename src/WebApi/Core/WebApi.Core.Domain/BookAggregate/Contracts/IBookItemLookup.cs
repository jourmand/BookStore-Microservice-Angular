using System.Threading.Tasks;

namespace WebApi.Core.Domain.BookAggregate.Contracts
{
    public interface IBookItemLookup
    {
        public Task<BookItem> FindByName(string name);
    }
}
