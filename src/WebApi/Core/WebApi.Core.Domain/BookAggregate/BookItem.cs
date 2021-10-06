using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;
using static BuildingBlocks.Framework.Entities.Events;
using static WebApi.Core.Domain.Exceptions.DomainExceptions;

namespace WebApi.Core.Domain.BookAggregate
{
    public class BookItem : AggregateRoot
    {
        public BookName Name { get; private set; }
        public BookText Text { get; private set; }
        public BookPrice Price { get; private set; }

        private HashSet<BookSubscription> _subscriptions = new HashSet<BookSubscription>();
        public IEnumerable<BookSubscription> Subscriptions => _subscriptions.ToList();

        public static BookItem Create(Guid id, BookName name, BookText text, BookPrice price)
        {
            var book = new BookItem();
            book.Apply(new BookCreatedIntegrationEvent
            {
                Id = id,
                Name = name,
                Price = price,
                Text = text
            });
            return book;
        }

        public void AddSubscription(ApplicationUserId userId)
        {
            if (Subscriptions.Any(o => o.ApplicationUserId == userId))
                throw new InvalidEntityState(
                    "User already subscribed to this book"
                );
            Apply(
                new SubscribeAddedToBookEvent
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = userId
                }
            );
        }

        public void Unsubscription(Guid userId)
        {
            var subscription = FindSubscription(userId);

            if (subscription == null)
                throw new InvalidEntityState(
                    "Cannot remove book I don't have"
                );
            Apply(new UnsubscribeToBookEvent
            {
                ApplicationUserId= userId,
                BookId = Id,
                Id = subscription.Id
            });
        }

        private BookSubscription FindSubscription(Guid id)
            => Subscriptions.FirstOrDefault(x => x.ApplicationUserId == id);

        protected override void When(object @event)
        {
            switch (@event)
            {
                case BookCreatedIntegrationEvent e:
                    Id = e.Id;
                    Name = new BookName(e.Name);
                    Price = new BookPrice(e.Price);
                    Text = new BookText(e.Text);
                    break;
                case SubscribeAddedToBookEvent e:
                    var subscription = new BookSubscription(Apply);
                    ApplyToEntity(subscription, e);
                    _subscriptions.Add(subscription);
                    break;
                case UnsubscribeToBookEvent e:
                    _subscriptions.Remove(FindSubscription(e.ApplicationUserId));
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            var valid =
                Name != null && Text != null;

            if (!valid)
                throw new DomainExceptions.InvalidEntityState(
                    $"User failed in state ");
        }
    }
}
