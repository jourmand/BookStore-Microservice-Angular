using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.WebApi.Infrastructure.Infrastructure;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Core.Domain.Exceptions;
using WebApi.Infrastructures.Data.Repositories;
using Xunit;

namespace Test.WebApi.Infrastructure
{
    public class RepositoryTests : DatabaseTestBase
    {
        private readonly BookItemRepository _testee;
        private readonly ApplicationUserLookup _applicationUserLookup;
        private readonly BookItem _newBookItem;

        public RepositoryTests()
        {
            _testee = new BookItemRepository(Context);
            _applicationUserLookup = new ApplicationUserLookup(Context);
            _newBookItem = BookItem.Create(new Guid("bf6d7876-8d93-4011-9acc-a3d496358c16"), new BookName("dotnet"), 
                new BookText(".net is one of my favorite framework"), new BookPrice(12));
        }

        [Theory]
        [InlineData("admin@live.com")]
        public void SubscribeBook_WhenUserIsNotNull_ShouldReturnBook(string email)
        {
            var bookItem = Context.BookItems
                .Include(o => o.Subscriptions)
                .First();
            bookItem.AddSubscription(ApplicationUserId.CreateByEmail(email, _applicationUserLookup));

            bookItem.Should().BeOfType<BookItem>();
            bookItem.Subscriptions.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("test@gmail.com")]
        public void SubscribeBook_WhenUserIsNull_ShouldReturnException(string email)
        {
            var bookItem = Context.BookItems
                .Include(o => o.Subscriptions)
                .First();

            Assert.Throws<DomainExceptions.InvalidEntityState>(() =>
                bookItem.AddSubscription(ApplicationUserId.CreateByEmail(email, _applicationUserLookup)));
        }

        [Fact]
        public void AddAsync_WhenEntityIsNull_ThrowsException()
        {
            _testee.Invoking(x => x.AddItem(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateBookAsync_WhenBookIsNotNull_ShouldReturnBook()
        {
            var result = await _testee.AddItem(_newBookItem);

            result.Should().BeOfType<BookItem>();
        }

        [Fact]
        public async void CreateBookAsync_WhenBookIsNotNull_ShouldAddBook()
        {
            var bookCount = Context.BookItems.Count();

            await _testee.AddItem(_newBookItem);

            await Context.SaveChangesAsync();
            Context.BookItems.Count().Should().Be(bookCount + 1);
        }
    }
}
