using System;

namespace BuildingBlocks.Framework.Entities
{
    public static class Events
    {
        public class UserCreatedIntegrationEvent : IDomainEvent
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class BookCreatedIntegrationEvent : IDomainEvent
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
            public decimal Price { get; set; }
        }

        public class SubscribeAddedToBookEvent : IDomainEvent
        {
            public Guid Id { get; set; }
            public Guid ApplicationUserId { get; set; }
        }

        public class UnsubscribeToBookEvent : IDomainEvent
        {
            public Guid Id { get; set; }
            public Guid BookId { get; set; }
            public Guid ApplicationUserId { get; set; }
        }
    }
    
}
