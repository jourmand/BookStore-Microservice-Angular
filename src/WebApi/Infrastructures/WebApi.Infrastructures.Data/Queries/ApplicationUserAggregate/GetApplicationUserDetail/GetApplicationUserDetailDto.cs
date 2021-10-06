using System;

namespace WebApi.Infrastructures.Data.Queries.ApplicationUserAggregate.GetApplicationUserDetail
{
    public class GetApplicationUserDetailDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
