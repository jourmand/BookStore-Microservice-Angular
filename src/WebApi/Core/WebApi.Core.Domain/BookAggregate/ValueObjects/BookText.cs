using System;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.BookAggregate.ValueObjects
{
    public class BookText : BaseValueObject<BookText>
    {
        public string Value { get; private set; }
        public BookText(string value)
        {
            Value = value;
        }

        public static BookText Create(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new DomainExceptions.InvalidEntityState("Text is required");
            if (text.Length < 15)
                throw new DomainExceptions.InvalidEntityState("Text length must grater than 15 character");
            return new BookText(text);
        }

        protected override int GetHashCodeCore() => Value.GetHashCode();

        protected override bool IsEqual(BookText other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

        public static implicit operator string(BookText value) => value.Value;
    }
}
