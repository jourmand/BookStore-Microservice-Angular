using System;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.UnsubscribeBookItem
{
    public class UnsubscribeBookItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
    }

}
