using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetUserBookSubscriptions
{
    public class GetUserBookSubscriptionsQueryHandler : IRequestHandler<GetUserBookSubscriptionsQuery, List<GetUserBookSubscriptionsDto>>
    {
        private readonly BookStoreDbContext _storeDbContext;

        public GetUserBookSubscriptionsQueryHandler(BookStoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public Task<List<GetUserBookSubscriptionsDto>> Handle(GetUserBookSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            return _storeDbContext.BookItems
                .SelectMany(o => o.Subscriptions.Where(o => o.ApplicationUser.Email == request.UserEmail), (books, subs) => new { books, subs } )
                .Select(o => new GetUserBookSubscriptionsDto
                {
                    Id = o.books.Id,
                    Text = o.books.Text,
                    Price = o.books.Price,
                    Name = o.books.Name
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
