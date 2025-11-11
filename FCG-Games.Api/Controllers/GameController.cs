using FCG_Games.Application.Games.Interfaces;
using FCG_Games.Application.Games.Requests;
using FCG_Games.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController(IGameService service) : ControllerBase
    {
        /// <summary>
        /// Cadastra um novo jogo.
        /// </summary>
        /// <param name="request">Dados necess�rios para o cadastro do jogo.</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>       
        [HttpPost]
        public async Task<IResult> CreateGameAsync([FromBody] GameRequest request, CancellationToken cancellation = default)
        {
            try
            {
                var result = await service.CreateGameAsync(request, cancellation);

                if (result.IsFailure)
                {
                    return result.Error.Code switch
                    {
                        "409" => TypedResults.Conflict(new Error("409", result.Error.Message)),
                        "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                        _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                    };
                }

                return TypedResults.Created($"/game/{result.Value.Id}", result.Value);

            }
            catch (Exception ex)
            {
                return TypedResults.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Busca um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser buscado</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>        
        [HttpGet("{id:guid}")]
        public async Task<IResult> GetGameByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            try
            {
                var result = await service.GetGameByIdAsync(id, cancellation);
                if (result.IsFailure)
                {
                    return result.Error.Code switch
                    {
                        "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                        _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                    };
                }
                return TypedResults.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Busca todos os jogos cadastrados.
        /// </summary>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>       
        [HttpGet]
        public async Task<IResult> GetAllGamesAsync(CancellationToken cancellation = default)
        {
            try
            {
                var result = await service.GetAllGamesAsync(cancellation);
                if (result.IsFailure)
                {
                    return TypedResults.BadRequest(new Error("400", result.Error.Message));
                }
                return TypedResults.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Remove um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>        
        [HttpDelete("{id:guid}")]
        public async Task<IResult> DeleteGameAsync(Guid id, CancellationToken cancellation = default)
        {
            try
            {
                var result = await service.DeleteGameAsync(id, cancellation);
                if (result.IsFailure)
                {
                    return result.Error.Code switch
                    {
                        "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                        _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                    };
                }
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Atualiza os dados de um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado </param>
        /// <param name="request">Novos dados que ser�o atribu�dos ao jogo</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>       
        [HttpPut("{id:guid}")]
        public async Task<IResult> UpdateGameAsync(Guid id, [FromBody] GameRequest request,  CancellationToken cancellation = default)
        {
            try
            {
                var result = await service.UpdateGameAsync(id, request, cancellation);
                if (result.IsFailure)
                {
                    return result.Error.Code switch
                    {
                        "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                        _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                    };
                }
                return TypedResults.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(detail: ex.Message, statusCode: 500);
            }
        }

    }
}
