using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItemDetail
{
    public class GetBookItemDetailQueryHandler : IRequestHandler<GetBookItemDetailQuery, GetBookItemDetailDto>
    {
        private readonly BookStoreDbContext _storeDbContext;

        public GetBookItemDetailQueryHandler(BookStoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public Task<GetBookItemDetailDto> Handle(GetBookItemDetailQuery request, CancellationToken cancellationToken)
        {
            return _storeDbContext.BookItems
                .Where(o => o.Id == request.Id)
                .Select(o => new GetBookItemDetailDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    Price = o.Price,
                    Name = o.Name
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
