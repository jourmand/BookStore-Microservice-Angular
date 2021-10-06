using System;

namespace WebApi.Core.Domain.Commons
{
    public abstract class Entity<TId> : IInternalEventHandler
        //where TId : Value<TId>
    {
        readonly Action<object> _applier;

        protected Entity(Action<object> applier) => _applier = applier;
        protected Entity() { }

        public TId Id { get; protected set; }

        void IInternalEventHandler.Handle(object @event) => When(@event);

        protected abstract void When(object @event);

        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }
    }
}
