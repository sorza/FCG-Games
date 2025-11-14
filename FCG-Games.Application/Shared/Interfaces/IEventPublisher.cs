using FCG_Games.Domain.Games.Enums;

namespace FCG_Games.Application.Shared.Interfaces
{
    public record GameCreatedEvent(Guid Id, string Title, decimal Price, int LaunchYear, string Developer, EGenre Genre);
    public record GameUpdatedEvent(Guid Id, string Title, decimal Price, int LaunchYear, string Developer, EGenre Genre);
    public record GameDeletedEvent(Guid Id);

    public interface IEventPublisher
    {
        Task PublishAsync<T>(T evt, string subject);
    }
}
