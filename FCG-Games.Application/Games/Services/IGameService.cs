using FCG_Games.Application.Games.Requests;
using FCG_Games.Application.Games.Responses;
using FCG_Games.Application.Shared.Results;

namespace FCG_Games.Application.Games.Services
{
    public interface IGameService
    {
        Task<Result<GameResponse>> CreateGameAsync(GameRequest request, CancellationToken cancellationToken = default);
        Task<Result<GameResponse>> GetGameByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<GameResponse>>> GetAllGamesAsync(CancellationToken cancellationToken = default);
        Task<Result> DeleteGameAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<GameResponse>> UpdateGameAsync(Guid id, GameRequest request, CancellationToken cancellationToken = default);
    }
}
