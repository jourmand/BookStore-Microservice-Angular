using System;
using System.Net;
using AutoMapper;
using Endpoints.WebApi.Controllers.v1;
using Endpoints.WebApi.ViewModels.v1.Book;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Infrastructures.Data.Queries.BookAggregate.GetBookItemDetail;
using Xunit;

namespace Test.WebApi.v1
{
    public class BookControllerTests
    {
        private readonly IMediator _mediator;
        private readonly BookController _testee;
        private readonly AddBookModel _createBookModel;
        private readonly Guid _id = Guid.Parse("5224ed94-6d9c-42ec-ba93-dfb11fe68931");

        public BookControllerTests()
        {
            var mapper = A.Fake<IMapper>();
            _mediator = A.Fake<IMediator>();
            _testee = new BookController(_mediator);

            _createBookModel = new AddBookModel
            {
                Name = "dotnet wcf",
                Price = (decimal)12.9,
                Text = "It is a framework used for developing SOAP"
            };
           
            var bookItem = BookItem.Create(_id, new BookName("dotnet wcf"), new BookText("It is a framework used for developing SOAP"), new BookPrice((decimal)12.9));
            var newBook = new CreateBookItemDto
            {
                Id = _id,
                Text = "dot net core one of the best stack of",
                Name = "C#",
                Price = 12
            };
            var getBookDetail = new GetBookItemDetailDto
            {
                Id = _id,
                Text = "dot net core one of the best stack of",
                Name = "C#",
                Price = 12
            };
            A.CallTo(() => mapper.Map<BookItem>(A<BookItem>._)).Returns(bookItem);
            A.CallTo(() => _mediator.Send(A<CreateBookItemCommand>._, default)).Returns(newBook);
            A.CallTo(() => _mediator.Send(A<GetBookItemDetailQuery>._, default)).Returns(getBookDetail);
        }

        [Fact]
        public async void Post_WhenAnExceptionOccurs_ShouldReturnBadRequest()
        {
            A.CallTo(() => _mediator.Send(A<CreateBookItemCommand>._, default)).Throws(new InvalidOperationException("message"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _testee.Post(_createBookModel));
        }

        [Fact]
        public async void Post_ShouldReturnBookItem()
        {
            var result = await _testee.Post(_createBookModel);

            var data = result as CreatedAtActionResult;

            var item = data?.Value as CreateBookItemDto;

            (result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.Created);
            result.Should().BeOfType<CreatedAtActionResult>();
            item?.Id.Should().Be(_id);
        }

        [Fact]
        public async void Get_ShouldReturnBook()
        {
            var result = await _testee.Get(_id.ToString());

            Assert.NotNull(result);
            Assert.IsType<GetBookItemDetailDto>(result);
            Assert.Equal(_id, result.Id);
        }
    }
}
