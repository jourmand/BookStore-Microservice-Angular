using System.Text.RegularExpressions;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects
{
    public class Email : Value<Email>
    {
        public string Value { get; private set; }
        public Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email,
            IApplicationUserLookup applicationLookup)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainExceptions.InvalidEntityState("Email is required");
            if (!Regex.IsMatch(email, "^(.+)@(.+)$"))
                throw new DomainExceptions.InvalidEntityState("Email not valid");
            if (applicationLookup.FindByEmail(email).GetAwaiter().GetResult() != null)
                throw new DomainExceptions.InvalidEntityState("Entered e-mail already exists");

            return new Email(email);
        }

        public static implicit operator string(Email value) => value.Value;
    }
}
