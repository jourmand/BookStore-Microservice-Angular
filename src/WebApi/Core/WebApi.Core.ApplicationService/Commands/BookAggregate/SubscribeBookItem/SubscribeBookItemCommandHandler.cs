using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.Commons;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.SubscribeBookItem
{
    public class SubscribeBookItemCommandHandler : IRequestHandler<SubscribeBookItemCommand, SubscribeBookItemDto>
    {
        private readonly IBookItemRepository _bookItemRepository;
        private readonly IApplicationUserLookup _applicationUserLookup;
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeBookItemCommandHandler(IBookItemRepository bookItemRepository,
            IApplicationUserLookup applicationUserLookup,
            IUnitOfWork unitOfWork)
        {
            _bookItemRepository = bookItemRepository;
            _applicationUserLookup = applicationUserLookup;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscribeBookItemDto> Handle(SubscribeBookItemCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookItemRepository.FindById(new Guid(request.BookId), cancellationToken);

            if (book == null)
                throw new InvalidOperationException("Book not found..");

            book.AddSubscription(ApplicationUserId.CreateByEmail(request.UserEmail, _applicationUserLookup));

            await _unitOfWork.SaveAsync(cancellationToken);
            return new SubscribeBookItemDto
            {
                Id = book.Id,
            };
        }
    }
}
