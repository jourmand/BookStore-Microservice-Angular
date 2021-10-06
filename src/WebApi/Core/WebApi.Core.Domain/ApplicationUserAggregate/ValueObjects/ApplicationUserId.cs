using System;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects
{
    public class ApplicationUserId : BaseValueObject<ApplicationUserId>
    {
        public Guid Value { get; private set; }
        public ApplicationUserId(Guid value)
        {
            Value = value;
        }

        public static ApplicationUserId CreateById(string userId,
            IApplicationUserLookup applicationLookup)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainExceptions.InvalidEntityState("UserId is required");
            
            if (applicationLookup.FindById(userId).GetAwaiter().GetResult() == null)
                throw new DomainExceptions.InvalidEntityState("Entered Id not exists");

            return new ApplicationUserId(new Guid(userId));
        }

        public static ApplicationUserId CreateByEmail(string email,
            IApplicationUserLookup applicationLookup)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainExceptions.InvalidEntityState("Email is required");
            var userDetail = applicationLookup.FindByEmail(email).GetAwaiter().GetResult();
            if (userDetail == null)
                throw new DomainExceptions.InvalidEntityState("Entered e-mail not exists");

            return new ApplicationUserId(userDetail.Id);
        }

        protected override int GetHashCodeCore() => Value.GetHashCode();

        protected override bool IsEqual(ApplicationUserId other) => Value == other.Value;

        public static implicit operator Guid(ApplicationUserId value) => value.Value;

    }
}
