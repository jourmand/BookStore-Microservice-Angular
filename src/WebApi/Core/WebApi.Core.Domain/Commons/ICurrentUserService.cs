
namespace WebApi.Core.Domain.Commons
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        string IpAddress { get; }
        string TransactionId { get; }
        bool? IsAdmin { get; }
    }
}
