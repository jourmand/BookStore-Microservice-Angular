using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.BookAggregate.ValueObjects
{
    public class BookPrice : BaseValueObject<BookPrice>
    {
        public decimal Value { get; private set; }
        public BookPrice(decimal value)
        {
            Value = value;
        }

        public static BookPrice Create(decimal price)
        {
            if (price <= 0)
                throw new DomainExceptions.InvalidEntityState("Price value not valid");
            return new BookPrice(price);
        }

        protected override int GetHashCodeCore() => Value.GetHashCode();

        protected override bool IsEqual(BookPrice other) => Value == other.Value;

        public static implicit operator decimal(BookPrice value) => value.Value;
    }
}
