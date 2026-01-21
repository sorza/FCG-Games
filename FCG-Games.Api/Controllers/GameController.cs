using FCG.Shared.Contracts.Results;
using FCG_Games.Application.Games.Requests;
using FCG_Games.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class GameController(IGameService service) : ControllerBase
    {
        /// <summary>
        /// Solicita o cadastro de um novo jogo.
        /// </summary>
        /// <param name="request">Dados necessários para o cadastro do jogo.</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisição.</param>       
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IResult> CreateGameAsync([FromBody] GameRequest request, CancellationToken cancellation = default)
        {            
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();
            var result = await service.CreateGameAsync(request, correlationId!, cancellation);

            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "409" => TypedResults.Conflict(new Error("409", result.Error.Message)),
                    _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                };
            }

            return TypedResults.Accepted($"/games/{result.Value.Id}", new {Game = result.Value, CorrelationId = correlationId});
        }

        /// <summary>
        /// Busca um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser buscado</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisição.</param>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IResult> GetGameByIdAsync(Guid id, CancellationToken cancellation = default)
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

        /// <summary>
        /// Busca todos os jogos cadastrados.
        /// </summary>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisiçãoo.</param>       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResult> GetAllGamesAsync(CancellationToken cancellation = default)
        {
            var result = await service.GetAllGamesAsync(cancellation);
            return TypedResults.Ok(result.Value);
        }

        /// <summary>
        /// Solicita a exclusão de um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisi��o.</param>        
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IResult> DeleteGameAsync(Guid id, CancellationToken cancellation = default)
        {
            
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();
            var result = await service.DeleteGameAsync(id, correlationId!, cancellation);

            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                    _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                };
            }

            return TypedResults.Accepted( $"/games/status/{correlationId}", new { GameId = id, CorrelationId = correlationId } );
        }

        /// <summary>
        /// Solicita atualização dos dados de um jogo pelo seu ID.
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado </param>
        /// <param name="request">Novos dados que serão atribu�dos ao jogo</param>
        /// <param name="cancellation">Token para monitorar o cancelamento da requisição.</param>    
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IResult> UpdateGameAsync(Guid id, [FromBody] GameRequest request,  CancellationToken cancellation = default)
        {           
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();
            var result = await service.UpdateGameAsync(id, request, correlationId!, cancellation);

            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    "404" => TypedResults.NotFound(new Error("404", result.Error.Message)),
                    "409" => TypedResults.Conflict(new Error("409", result.Error.Message)),
                    _ => TypedResults.BadRequest(new Error("400", result.Error.Message))
                };
            }

            return TypedResults.Accepted($"/games/status/{correlationId}", new { Game = result.Value, CorrelationId = correlationId });
        }
    }
}
