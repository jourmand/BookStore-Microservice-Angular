using System.Collections.Generic;
using MediatR;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItems
{
    public class GetBookItemsQuery : IRequest<List<GetBookItemsDto>>
    {
    }
}
