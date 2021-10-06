using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using MediatR;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.Commons;

namespace WebApi.Core.ApplicationService.Commands.ApplicationUserAggregate.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandDto>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork)
        {
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserCommandDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserRepository.Register(new Events.UserCreatedIntegrationEvent
            {
                Email = request.Email,
                FirstName = request.FirstName,
                Password = request.Password,
                LastName = request.LastName
            }, cancellationToken);
                
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateUserCommandDto
            {
                Id = result.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
        }
    }
}
