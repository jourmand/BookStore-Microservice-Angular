using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.BookAggregate.ValueObjects
{
    public class BookText : Value<BookText>
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

        public static implicit operator string(BookText value) => value.Value;
    }
}
