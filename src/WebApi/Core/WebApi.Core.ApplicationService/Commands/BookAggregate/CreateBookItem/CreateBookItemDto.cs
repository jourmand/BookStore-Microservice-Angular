using System;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem
{
    public class CreateBookItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
    }

}
