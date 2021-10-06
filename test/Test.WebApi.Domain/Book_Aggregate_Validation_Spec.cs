using Moq;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Core.Domain.Exceptions;
using Xunit;

namespace Test.WebApi.Domain
{
    public class Book_Aggregate_Validation_Spec
    {
        [Theory]
        [InlineData("Asp.Net")]
        [InlineData("dot.Net")]
        public void Book_Should_Validate(string value)
        {
            var bookLookup = new Mock<IBookItemLookup>();
            var bookName = BookName.Create(value, bookLookup.Object);

            Assert.Equal(value, bookName.Value);
        }

        [Fact]
        public void Book_Exist_Validate()
        {
            var bookLookup = new Mock<IBookItemLookup>();
            bookLookup.Setup(m => m.FindByName("asp.net")).ReturnsAsync(new BookItem());
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => BookName.Create("asp.net", bookLookup.Object));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Book_Name_Format_Validate(string value)
        {
            var bookLookup = new Mock<IBookItemLookup>();
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => BookName.Create(value, bookLookup.Object));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Book_Text_Not_Validate(string value)
        {
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => BookText.Create(value));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(null)]
        public void Book_Price_Not_Validate(decimal value)
        {
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => BookPrice.Create(value));
        }
    }
}
