using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;

namespace WebApi.Core.Domain.ApplicationUserAggregate.Contracts
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUserInfo> Register(Events.UserCreatedIntegrationEvent user, CancellationToken cancellationToken = default);
        Task<ApplicationUserInfo> FindByEmail(string email, CancellationToken cancellationToken = default);

    }
}
