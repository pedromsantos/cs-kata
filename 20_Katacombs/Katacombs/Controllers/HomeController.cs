using Microsoft.AspNetCore.Mvc;

namespace Katacombs.Controllers;

/// <summary>
/// Home controller for swagger documentation requests.
/// </summary>
public class HomeController : ControllerBase
{
	/// <summary>
	/// Redirect root requests to Swagger API documentation.
	/// </summary>
	/// <returns>Redirect to Swagger API documentation.</returns>
	[ApiExplorerSettings(IgnoreApi = true)]
	[HttpGet("/")]
	public IActionResult Home()
	{
		return RedirectPermanent("/swagger/index.html");
	}

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
