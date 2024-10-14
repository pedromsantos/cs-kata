namespace Katacombs.Controllers.Responses;

/// <summary>
/// Represents a response containing player information.
/// </summary>
/// <param name="Sid">The player's session ID.</param>
/// <param name="Name">The player's name.</param>
public record PlayersResponse(string Sid, string Name);
