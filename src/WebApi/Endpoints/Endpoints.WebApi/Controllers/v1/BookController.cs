using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using Endpoints.WebApi.ViewModels.v1.Book;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem;
using WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItemDetail;
using WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItems;

namespace Endpoints.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "seller")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateBookItemDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post(AddBookModel model)
        {
            var result = await _mediator.Send(new CreateBookItemCommand
            {
                Name = model.Name,
                Text= model.Text,
                Price = model.Price
            });
            return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetBookItemDetailDto), (int)HttpStatusCode.OK)]
        public async Task<GetBookItemDetailDto> Get(string id)
        {
            var result = await _mediator.Send(new GetBookItemDetailQuery(new Guid(id)));
            return result;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<GetBookItemsDto>), (int)HttpStatusCode.OK)]
        public async Task<List<GetBookItemsDto>> Books()
        {
            var result = await _mediator.Send(new GetBookItemsQuery());
            return result;
        }
    }
}