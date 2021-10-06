namespace WebApi.Core.Domain.Commons
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}