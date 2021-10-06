using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructures.Data.Commons;

namespace WebApi.Infrastructures.Data.Queries.ApplicationUserAggregate.GetApplicationUserDetail
{
    public class GetApplicationUserDetailQueryHandler : IRequestHandler<GetApplicationUserDetailQuery, GetApplicationUserDetailDto>
    {
        private readonly BookStoreDbContext _storeDbContext;

        public GetApplicationUserDetailQueryHandler(BookStoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public Task<GetApplicationUserDetailDto> Handle(GetApplicationUserDetailQuery request, CancellationToken cancellationToken)
        {
            return _storeDbContext.ApplicationUserInfos
                .Where(o => (!request.Id.HasValue || o.Id == request.Id) &&
                            (string.IsNullOrEmpty(request.Email) || o.Email == request.Email))
                .Select(o => new GetApplicationUserDetailDto
                {
                    Id = o.Id,
                    Email = o.Email,
                    FirstName = o.FullName.FirstName,
                    LastName = o.FullName.LastName,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
