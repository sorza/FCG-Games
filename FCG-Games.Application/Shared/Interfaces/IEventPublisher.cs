using FCG_Games.Domain.Games.Enums;

namespace FCG_Games.Application.Shared.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T evt, string subject);
    }
}
