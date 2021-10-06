using System.Collections.Generic;

namespace WebApi.Core.Domain.Commons
{
    public interface IDomainEventHandlingExecutor
    {
        void Execute(IEnumerable<AggregateRoot> domainEventEntities);
    }
}
