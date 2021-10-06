using System;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetUserBookSubscriptions
{
    public class GetUserBookSubscriptionsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
    }
}
