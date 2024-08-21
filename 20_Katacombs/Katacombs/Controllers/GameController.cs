using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Katacombs.Controllers.Requests;

namespace Katacombs.Controllers
{
    [ApiController]
    [SwaggerTag("Game operations")]
    public class GameController : ControllerBase
    {
        /// <summary>
        /// Start a new game
        /// </summary>
        /// <remarks>Starts a Game for a new player</remarks>
        /// <param name="body"></param>
        /// <response code="201">Game started for player</response>
        /// <response code="400">Game not started for player</response>
        [HttpPost]
        [Route("/game/player")]
        [SwaggerOperation(
            Summary = "Start Game",
            Description = "Starts a Game for a new player",
            OperationId = "CreatePlayer",
            Tags = ["Game", "Player"]
        )]
        [SwaggerResponse(StatusCodes.Status201Created, description: "Created Game", typeof(void))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, description: "Bad Request", typeof(void))]
        public virtual IActionResult CreatePlayer([FromBody, SwaggerRequestBody("The player payload", Required = true)] PlayerRequest body)
        {
            throw new NotImplementedException();
        }

        // /// <summary>
        // /// Help
        // /// </summary>
        // /// <remarks>Returns avaible commands and game hints for player</remarks>
        // /// <param name="playerSid">Player unique identification</param>
        // /// <response code="200">Available commands and hints returned to caller</response>
        // [HttpGet]
        // [Route("/game/player/{playerSid}")]
        // [SwaggerOperation("GetHelpAndHints")]
        // [SwaggerResponse(statusCode: 200, type: typeof(HelpResponse), description: "Available commands and hints returned to caller")]
        // public virtual IActionResult GetHelpAndHints([FromRoute][Required] string playerSid)
        // {
        //     throw new NotImplementedException();
        // }

        // /// <summary>
        // /// list of active players
        // /// </summary>
        // /// <remarks>Returns the list of active players</remarks>
        // /// <response code="200">list of players</response>
        // [HttpGet]
        // [Route("/game/player")]
        // [SwaggerOperation("ListPlayers")]
        // [SwaggerResponse(statusCode: 200, type: typeof(PlayersResponse), description: "list of players")]
        // public virtual IActionResult ListPlayers()
        // {
        //     throw new NotImplementedException();
        // }

        // /// <summary>
        // /// Quit Game
        // /// </summary>
        // /// <remarks>By removing the user she quits the game</remarks>
        // /// <param name="playerSid">Player unique identification</param>
        // /// <response code="200">Game finshed</response>
        // /// <response code="404">Player with Sid playerSid not found</response>
        // [HttpDelete]
        // [Route("/game/player/{playerSid}")]
        // [SwaggerOperation("QuitGame")]
        // public virtual IActionResult QuitGame([FromRoute][Required] string playerSid)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
