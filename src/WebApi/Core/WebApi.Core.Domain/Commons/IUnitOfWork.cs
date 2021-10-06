using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Core.Domain.Commons
{
    public interface IUnitOfWork
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
        void Save();
    }
}