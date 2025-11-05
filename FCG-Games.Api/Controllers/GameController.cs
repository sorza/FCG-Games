using FCG_Games.Application.Games.Interfaces;
using FCG_Games.Application.Games.Requests;
using FCG_Games.Application.Games.Responses;
using FCG_Games.Application.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController(IGameService service) : ControllerBase
    {

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
