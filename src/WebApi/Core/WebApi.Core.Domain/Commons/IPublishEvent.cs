using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Core.Domain.Commons
{
    public interface IPublishEvent
    {
        public Task Publish<T>(T @event);
        public Task Publish<T>(T @event, CancellationToken cancellationToken);
    }
}
