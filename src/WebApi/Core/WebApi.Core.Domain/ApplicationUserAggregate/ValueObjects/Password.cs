using System.Text.RegularExpressions;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects
{
    public class Password : Value<Password>
    {
        public string Value { get; private set; }
        public Password(string value)
        {
            Value = value;
        }

        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainExceptions.InvalidEntityState("Password is required");
            if (!Regex.IsMatch(password, "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*]).{6,}$"))
                throw new DomainExceptions.InvalidEntityState("Password should contain uppercase, lowercase, numeric & signs");

            return new Password(password);
        }

        public static implicit operator string(Password value) => value.Value;
    }
}
