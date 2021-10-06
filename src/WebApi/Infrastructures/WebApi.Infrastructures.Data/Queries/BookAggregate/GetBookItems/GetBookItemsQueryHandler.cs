using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItems
{
    public class GetBookItemsQueryHandler : IRequestHandler<GetBookItemsQuery, List<GetBookItemsDto>>
    {
        private readonly BookStoreDbContext _storeDbContext;

        public GetBookItemsQueryHandler(BookStoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public Task<List<GetBookItemsDto>> Handle(GetBookItemsQuery request, CancellationToken cancellationToken)
        {
            return _storeDbContext.BookItems
                .Select(o => new GetBookItemsDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    Price = o.Price,
                    Name = o.Name
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
