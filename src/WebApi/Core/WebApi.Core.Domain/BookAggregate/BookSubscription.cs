using System;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.Commons;
using static BuildingBlocks.Framework.Entities.Events;

namespace WebApi.Core.Domain.BookAggregate
{
    public class BookSubscription : Entity<Guid>
    {
        public BookSubscription(Action<object> applier) : base(applier) { }
        private BookSubscription() { }

        public ApplicationUserInfo ApplicationUser { get; private set; }
        public Guid ApplicationUserId { get; private set; }
        
        protected override void When(object @event)
        {
            switch (@event)
            {
                case SubscribeAddedToBookEvent e:
                    ApplicationUserId = e.ApplicationUserId;
                    Id = e.Id;
                    break;
            }
        }
    }
}
