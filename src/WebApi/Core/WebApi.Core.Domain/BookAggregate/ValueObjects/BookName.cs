using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.BookAggregate.ValueObjects
{
    public class BookName : Value<BookName>
    {
        public string Value { get; private set; }
        public BookName(string value)
        {
            Value = value;
        }

        public static BookName Create(string name,
            IBookItemLookup bookItemLookup)
        {
            DataValidation(name, bookItemLookup);
            return new BookName(name);
        }

        private static void DataValidation(string name, IBookItemLookup bookItemLookup)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainExceptions.InvalidEntityState("Name is required");
            if (bookItemLookup.FindByName(name).GetAwaiter().GetResult() != null)
                throw new DomainExceptions.InvalidEntityState("Entered name already exists");
        }

        //protected override int GetHashCodeCore() => Value.GetHashCode();

        //protected override bool IsEqual(BookName other) => Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);

        public static implicit operator string(BookName value) => value.Value;
    }
}
