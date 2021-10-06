using System;
using MediatR;

namespace WebApi.Infrastructures.Data.Queries.ApplicationUserAggregate.GetApplicationUserDetail
{
    public class GetApplicationUserDetailQuery : IRequest<GetApplicationUserDetailDto>
    {
        public GetApplicationUserDetailQuery(Guid id)
        {
            Id = id;
        }

        public GetApplicationUserDetailQuery(string email)
        {
            Email = email;
        }
        public Guid? Id { get; }
        public string Email { get; }
    }
}
