namespace Katacombs.Controllers.Requests;

/// <summary>
/// Represents a request for a player.
/// </summary>
/// <param name="Sid">The session ID of the player.</param>
/// <param name="Name">The name of the player.</param>
public record PlayerRequest(string Sid, string Name);