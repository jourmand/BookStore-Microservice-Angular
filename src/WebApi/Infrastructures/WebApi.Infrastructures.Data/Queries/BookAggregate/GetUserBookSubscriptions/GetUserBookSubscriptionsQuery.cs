using System.Collections.Generic;
using MediatR;

namespace WebApi.Infrastructures.Data.Queries.BookAggregate.GetUserBookSubscriptions
{
    public class GetUserBookSubscriptionsQuery : IRequest<List<GetUserBookSubscriptionsDto>>
    {
        public GetUserBookSubscriptionsQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
        public string UserEmail { get; set; }
    }
}
