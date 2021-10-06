using System;
using MediatR;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItemDetail
{
    public class GetBookItemDetailQuery : IRequest<GetBookItemDetailDto>
    {
        public GetBookItemDetailQuery(Guid id)
        {
            Id = id;
        }

        public Guid? Id { get; }
    }
}
