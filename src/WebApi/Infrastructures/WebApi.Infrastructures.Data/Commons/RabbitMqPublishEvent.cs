using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using WebApi.Core.Domain.Commons;

namespace WebApi.Infrastructures.Data.Commons
{
    public class RabbitMqPublishEvent : IPublishEvent
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqPublishEvent(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish<T>(T @event) =>
            await _publishEndpoint.Publish(@event);

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) =>
            await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
