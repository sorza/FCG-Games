using FCG_Games.Domain.Games.Entities;

namespace FCG_Games.Application.Shared.Interfaces.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<bool> Exists(Game game, CancellationToken cancellationToken = default);
    }
}
