using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects
{
    public class FullName : Value<FullName>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public FullName() { }

        public static FullName Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainExceptions.InvalidEntityState("FirstName is required");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainExceptions.InvalidEntityState("LastName is required");
            if (firstName.Length > 100)
                throw new DomainExceptions.InvalidEntityState("FirstName not more than 100 character.");
            if (lastName.Length > 100)
                throw new DomainExceptions.InvalidEntityState("LastName not more than 100 character.");
            
            return new FullName(firstName, lastName);
        }

        public override string ToString() =>
            $"{FirstName ?? ""} {LastName ?? ""}";
    }
}
