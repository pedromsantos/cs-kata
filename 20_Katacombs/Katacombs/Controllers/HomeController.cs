using Microsoft.AspNetCore.Mvc;

namespace Katacombs.Controllers;

/// <summary>
/// Home controller for handling requests.
/// </summary>
public class HomeController : ControllerBase
{
	/// <summary>
	/// Swagger API documentation.
	/// </summary>
	/// <returns>Displays Swagger API documentation.</returns>
	[ApiExplorerSettings(IgnoreApi = true)]
	[HttpGet("/swagger")]
	public IActionResult Swagger()
	{
		return RedirectPermanent("/swagger/index.html");
	}
}
