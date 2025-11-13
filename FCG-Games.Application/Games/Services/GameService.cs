using FCG_Games.Application.Games.Requests;
using FCG_Games.Application.Games.Responses;
using FCG_Games.Application.Shared.Interfaces.Repositories;
using FCG_Games.Application.Shared.Results;
using FCG_Games.Domain.Games.Entities;
using FluentValidation;

namespace FCG_Games.Application.Games.Services
{
    public class GameService(IGameRepository repository, 
                             IValidator<GameRequest> validator) : IGameService
    {
        public async Task<Result<GameResponse>> CreateGameAsync(GameRequest request, CancellationToken cancellationToken = default)
        {
            var validation = validator.Validate(request);
            if(!validation.IsValid)    
                return Result.Failure<GameResponse>(new Error("400", string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

            var game = Game.Create(request.Title, request.Price, request.LaunchYear, request.Developer, request.Genre);
           
            if(await repository.Exists(game))
                return Result.Failure<GameResponse>(new Error("409", "Este jogo já está cadastrado"));

            await repository.AddAsync(game, cancellationToken);
            
            return Result.Success(Parse(game));

        }

        public async Task<Result> DeleteGameAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var game = await GetGameByIdAsync(id, cancellationToken);

            await repository.DeleteAsync(id, cancellationToken);

            return Result.Success(game);

        }

        public async Task<Result<IEnumerable<GameResponse>>> GetAllGamesAsync(CancellationToken cancellationToken = default)
        {
            var games = await repository.GetAllAsync();

            var response = games.Select(Parse).ToList();
            
            return Result.Success<IEnumerable<GameResponse>>(response);

        }

        public async Task<Result<GameResponse>> GetGameByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           var game = await repository.GetByIdAsync(id, cancellationToken);

            if(game is null)
                return Result.Failure<GameResponse>(new Error("404", "Jogo não encontrado"));

            return Result.Success(Parse(game));
        }

        public async Task<Result<GameResponse>> UpdateGameAsync(Guid id, GameRequest request, CancellationToken cancellationToken = default)
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
                return Result.Failure<GameResponse>(new Error("400", string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

            var game = await repository.GetByIdAsync(id, cancellationToken);
            if(game is null)
                return Result.Failure<GameResponse>(new Error("404", "Jogo não encontrado"));

            game.Update(request.Title, request.Price, request.LaunchYear, request.Developer, request.Genre);
            await repository.UpdateAsync(game, cancellationToken);

            return Result.Success(Parse(game));
        }

        public static Game Parse(GameRequest request)
        => Game.Create(request.Title, request.Price, request.LaunchYear, request.Developer, request.Genre);        

        public static GameResponse Parse(Game game)
        => new GameResponse(game.Id, game.Title, game.Price, game.LaunchYear, game.Developer, game.Genre);        
    }
}
