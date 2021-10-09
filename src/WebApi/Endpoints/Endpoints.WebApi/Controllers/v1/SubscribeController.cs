using Endpoints.WebApi.ViewModels.v1.Subscribe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem;
using WebApi.Core.ApplicationService.Commands.BookAggregate.SubscribeBookItem;
using WebApi.Core.ApplicationService.Commands.BookAggregate.UnsubscribeBookItem;
using WebApi.Infrastructures.Data.Queries.BookAggregate.GetUserBookSubscriptions;

namespace Endpoints.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscribeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscribeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateBookItemDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post(SubscribeBookModel model)
        {
            var result = await _mediator.Send(new SubscribeBookItemCommand(model.BookId, User.Identity?.Name));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new UnsubscribeBookItemCommand(id, User.Identity?.Name));
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<GetUserBookSubscriptionsDto>), (int)HttpStatusCode.OK)]
        public async Task<List<GetUserBookSubscriptionsDto>> Subscribes()
        {
            var result = await _mediator.Send(new GetUserBookSubscriptionsQuery(User.Identity?.Name));
            return result;
        }
    }
}