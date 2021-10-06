using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Core.Domain.Commons;

namespace WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem
{
    public class CreateBookItemCommandHandler : IRequestHandler<CreateBookItemCommand, CreateBookItemDto>
    {
        private readonly IBookItemRepository _bookItemRepository;
        private readonly IBookItemLookup _bookItemLookup;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookItemCommandHandler(IBookItemRepository bookItemRepository,
            IBookItemLookup bookItemLookup,
            IUnitOfWork unitOfWork)
        {
            _bookItemRepository = bookItemRepository;
            _bookItemLookup = bookItemLookup;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateBookItemDto> Handle(CreateBookItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookItemRepository.AddItem(BookItem.Create(Guid.NewGuid(), BookName.Create(request.Name, _bookItemLookup),
                BookText.Create(request.Text), BookPrice.Create(request.Price)), cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return new CreateBookItemDto
            {
                Id = result.Id,
                Name = result.Name,
                Price = result.Price,
                Text = result.Text
            };
        }
    }
}
