// using Microsoft.AspNetCore.Mvc;
// using Swashbuckle.AspNetCore.Annotations;
// using System.ComponentModel.DataAnnotations;
// using Katacombs.Controllers.Responses;

// namespace Katacombs.Controllers
// {
//     [ApiController]
//     public class ItemsController : ControllerBase
//     {
//         /// <summary>
//         /// Inspect item
//         /// </summary>
//         /// <remarks>Returns more detailed information about an item</remarks>
//         /// <param name="itemSid"></param>
//         /// <response code="200">Item desctiption</response>
//         /// <response code="404">Item with Sid not found</response>
//         [HttpGet]
//         [Route("/items/{itemSid}")]
//         [SwaggerOperation("GetItemDescription")]
//         [SwaggerResponse(statusCode: 200, type: typeof(ItemResponse), description: "Item desctiption")]
//         public virtual IActionResult GetItemDescription([FromRoute][Required] string itemSid)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }
