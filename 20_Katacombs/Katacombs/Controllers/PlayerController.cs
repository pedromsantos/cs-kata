using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Katacombs.Controllers.Requests;
using Katacombs.Controllers.Responses;

namespace Katacombs.Controllers
{
    [ApiController]
    public class PlayerController : ControllerBase
    {
        /// <summary>
        /// Bag content
        /// </summary>
        /// <remarks>Returns the current list of items in player&#x27;s bag</remarks>
        /// <param name="playerSid">Player unique identification</param>
        /// <response code="200">Bag content</response>
        /// <response code="404">Player with Sid playerSid not found</response>
        [HttpGet]
        [Route("/player/{playerSid}/bag")]
        [SwaggerOperation("GetBag")]
        [SwaggerResponse(statusCode: 200, type: typeof(BagResponse), description: "Bag content")]
        public virtual IActionResult GetBag([FromRoute][Required] string playerSid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Look at specific direction
        /// </summary>
        /// <remarks>Returns information about a location in a specific direction</remarks>
        /// <param name="playerSid">Player unique identification</param>
        /// <param name="direction"></param>
        /// <response code="200">Location description for a direction</response>
        /// <response code="404">Player with Sid playerSid not found</response>
        [HttpGet]
        [Route("/player/{playerSid}/look/{direction}")]
        [SwaggerOperation("GetDirectionDescription")]
        [SwaggerResponse(statusCode: 200, type: typeof(LocationResponse), description: "Location description for a direction")]
        public virtual IActionResult GetDirectionDescription([FromRoute][Required] string playerSid, [FromRoute][Required] DirectionRequest direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Player location
        /// </summary>
        /// <remarks>Returns information about a location</remarks>
        /// <param name="playerSid">Player unique identification</param>
        /// <response code="200">Room description</response>
        /// <response code="404">Player with Sid playerSid not found</response>
        [HttpGet]
        [Route("/player/{playerSid}/location")]
        [SwaggerOperation("LookArround")]
        [SwaggerResponse(statusCode: 200, type: typeof(LocationResponse), description: "Room description")]
        public virtual IActionResult LookArround([FromRoute][Required] string playerSid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Move
        /// </summary>
        /// <remarks>Move in specified direction</remarks>
        /// <param name="playerSid">Player unique identification</param>
        /// <param name="direction"></param>
        /// <response code="200">Moved in requested direction</response>
        /// <response code="405">Cannot move in requested direction</response>
        [HttpPut]
        [Route("/player/{playerSid}/move/{direction}")]
        [SwaggerOperation("Move")]
        public virtual IActionResult Move([FromRoute][Required] string playerSid, [FromRoute][Required] DirectionRequest direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use an item
        /// </summary>
        /// <param name="playerSid">Player unique identification</param>
        /// <param name="itemSid"></param>
        /// <param name="action"></param>
        /// <response code="200">Action performed</response>
        /// <response code="404">Player or Item with Sid playerSid not found</response>
        /// <response code="405">Action not available for specified item</response>
        [HttpPut]
        [Route("/player/{playerSid}/item/{itemSid}/use/{action}")]
        [SwaggerOperation("UseItem")]
        public virtual IActionResult UseItem([FromRoute][Required] string playerSid, [FromRoute][Required] string itemSid, [FromRoute][Required] Action action)
        {
            throw new NotImplementedException();
        }
    }
}
