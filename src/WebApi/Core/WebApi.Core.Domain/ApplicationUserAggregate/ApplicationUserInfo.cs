using System;
using BuildingBlocks.Framework.Entities;
using WebApi.Core.Domain.ApplicationUserAggregate.ValueObjects;
using WebApi.Core.Domain.Commons;
using WebApi.Core.Domain.Exceptions;

namespace WebApi.Core.Domain.ApplicationUserAggregate
{
    public class ApplicationUserInfo : AggregateRoot
    {
        public ApplicationUserInfo(){ }
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }

        public static ApplicationUserInfo Create(Guid id, FullName fullName, Email email, Password password)
        {
            var user = new ApplicationUserInfo();
            user.Apply(new Events.UserCreatedIntegrationEvent
            {
                Id = id,
                Email = email,
                FirstName = fullName.FirstName,
                LastName = fullName.LastName,
                Password = password
            });
            return user;
        }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.UserCreatedIntegrationEvent e:
                    Id = e.Id;
                    Email = new Email(e.Email);
                    FullName = FullName.Create(e.FirstName, e.LastName);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            var valid =
                FullName != null && Email != null;

            if (!valid)
                throw new DomainExceptions.InvalidEntityState(
                    "User failed in state ");
        }
    }
}
