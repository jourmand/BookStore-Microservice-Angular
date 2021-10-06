using System.Threading.Tasks;

namespace WebApi.Core.Domain.ApplicationUserAggregate.Contracts
{
    public interface IApplicationUserLookup
    {
        public Task<ApplicationUserInfo> FindByEmail(string email);
        public Task<ApplicationUserInfo> FindById(string id);
    }
}
