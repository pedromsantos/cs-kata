using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Katacombs.Controllers.Requests;
using Katacombs.Controllers.Responses;

namespace Katacombs.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        /// <summary>
        /// Start a new game
        /// </summary>
        /// <remarks>Show description</remarks>
        /// <param name="body"></param>
        /// <response code="201">Game started</response>
        [HttpPost]
        [Route("/game/player")]
        [SwaggerOperation("CreatePlayer")]
        public virtual IActionResult CreatePlayer([FromBody] PlayerRequest body)
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
