using MediatR;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.SubscribeBookItem
{
    public class SubscribeBookItemCommand : IRequest<SubscribeBookItemDto>
    {
        public SubscribeBookItemCommand(string bookId, string userEmail)
        {
            BookId = bookId;
            UserEmail = userEmail;
        }
        public string BookId { get; }
        public string UserEmail { get; }
    }
}
