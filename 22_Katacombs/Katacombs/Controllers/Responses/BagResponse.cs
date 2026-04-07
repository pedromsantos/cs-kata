namespace Katacombs.Controllers.Responses;

/// <summary>
/// Represents a response containing a list of direction responses.
/// </summary>
/// <param name="Items">The list of direction responses.</param>
public record BagResponse(
    List<DirectionResponse> Items);