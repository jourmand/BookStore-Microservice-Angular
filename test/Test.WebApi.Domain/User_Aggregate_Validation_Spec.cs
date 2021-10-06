using Moq;
using WebApi.Core.Domain.ApplicationUserAggregate;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using Xunit;
using static WebApi.Core.Domain.Exceptions.DomainExceptions;

namespace Test.WebApi.Domain
{
    public class User_Aggregate_Validation_Spec
    {
        [Fact]
        public void Email_Should_Validate()
        {
            var applicationLookupRepository = new Mock<IApplicationUserLookup>();

            var email = Email.Create("admin@test.com", applicationLookupRepository.Object);

            Assert.Equal("admin@test.com", email.Value);
        }

        [Fact]
        public void Email_Exist_Validate()
        {
            var applicationLookupRepository = new Mock<IApplicationUserLookup>();
            applicationLookupRepository.Setup(m => m.FindByEmail("admin@test.com")).ReturnsAsync(new ApplicationUserInfo());
            Assert.Throws<InvalidEntityState>(() => Email.Create("admin@test.com", applicationLookupRepository.Object));
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("")]
        public void Email_Format_Validate(string value)
        {
            var applicationLookupRepository = new Mock<IApplicationUserLookup>();
            Assert.Throws<InvalidEntityState>(() => Email.Create(value, applicationLookupRepository.Object));
        }

        [Theory]
        [InlineData("Rasoul", "Jourmand", "Rasoul Jourmand")]
        [InlineData("Max", "Irvin", "Max Irvin")]
        public void FullName_Is_Valid(string firstName, string lastName, string expected)
        {
            var fullName = FullName.Create(firstName, lastName);
            Assert.Equal(expected, fullName.ToString());
        }

        [Theory]
        [InlineData("Q12@ss")]
        [InlineData("1234&Bwf")]
        public void Password_Is_Valid(string value)
        {
            var result = Password.Create(value);
            Assert.Equal(value, result.Value);
        }

        [Theory]
        [InlineData("Q12ss")]
        [InlineData("ssss&w")]
        public void Password_Not_Valid(string value)
        {
            Assert.Throws<InvalidEntityState>(() => Password.Create(value));
        }
    }
}
