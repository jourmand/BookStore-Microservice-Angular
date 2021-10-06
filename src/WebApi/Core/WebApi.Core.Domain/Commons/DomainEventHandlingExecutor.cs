using System.Collections.Generic;

namespace WebApi.Core.Domain.Commons
{
    public class DomainEventHandlingExecutor : IDomainEventHandlingExecutor
    {
        private readonly IPublishEvent _publishEvent;

        public DomainEventHandlingExecutor(IPublishEvent publishEvent)
        {
            _publishEvent = publishEvent;
        }

        public void Execute(IEnumerable<AggregateRoot> domainEventEntities)
        {
            foreach (var entity in domainEventEntities)
            {
                foreach (var @event in entity.GetChanges())
                {
                    _publishEvent.Publish(@event);
                }
                entity.ClearChanges();
            }
        }
    }
}
