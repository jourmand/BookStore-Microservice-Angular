using MediatR;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.UnsubscribeBookItem
{
    public class UnsubscribeBookItemCommand : IRequest<UnsubscribeBookItemDto>
    {
        public UnsubscribeBookItemCommand(string bookId, string userEmail)
        {
            BookId = bookId;
            UserEmail = userEmail;
        }
        public string BookId { get; }
        public string UserEmail { get; }
    }
}
