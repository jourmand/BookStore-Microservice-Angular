using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using Endpoints.WebApi.ViewModels.v1.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.ApplicationService.Commands.ApplicationUserAggregate.CreateUser;

namespace Endpoints.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateUserCommandDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            var result = await _mediator.Send(new CreateUserCommand
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            });

            return Ok(result);
        }
    }
}