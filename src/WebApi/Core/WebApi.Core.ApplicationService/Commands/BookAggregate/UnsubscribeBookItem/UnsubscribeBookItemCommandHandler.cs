using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.Commons;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.UnsubscribeBookItem
{
    public class UnsubscribeBookItemCommandHandler : IRequestHandler<UnsubscribeBookItemCommand, UnsubscribeBookItemDto>
    {
        private readonly IBookItemRepository _bookItemRepository;
        private readonly IApplicationUserLookup _applicationUserLookup;
        private readonly IUnitOfWork _unitOfWork;

        public UnsubscribeBookItemCommandHandler(IBookItemRepository bookItemRepository,
            IApplicationUserLookup applicationUserLookup,
            IUnitOfWork unitOfWork)
        {
            _bookItemRepository = bookItemRepository;
            _applicationUserLookup = applicationUserLookup;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnsubscribeBookItemDto> Handle(UnsubscribeBookItemCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookItemRepository.FindById(new Guid(request.BookId), cancellationToken);
            if (book == null)
                throw new InvalidOperationException("Book not found..");

            book.Unsubscription(ApplicationUserId.CreateByEmail(request.UserEmail, _applicationUserLookup));
            await _unitOfWork.SaveAsync(cancellationToken);
            return new UnsubscribeBookItemDto
            {
                Id= book.Id,
            };
        }
    }
}
