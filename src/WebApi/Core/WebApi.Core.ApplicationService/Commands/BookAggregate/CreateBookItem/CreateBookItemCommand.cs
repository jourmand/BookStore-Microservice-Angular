using MediatR;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem
{
    public class CreateBookItemCommand : IRequest<CreateBookItemDto>
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
    }
}
