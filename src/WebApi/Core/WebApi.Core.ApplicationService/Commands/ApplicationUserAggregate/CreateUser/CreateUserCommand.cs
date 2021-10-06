using MediatR;

namespace WebApi.Core.ApplicationService.Commands.ApplicationUserAggregate.CreateUser
{
    public class CreateUserCommand : IRequest<CreateUserCommandDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
