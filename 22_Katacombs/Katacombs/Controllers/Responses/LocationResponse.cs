using Katacombs.Controllers.Requests;

namespace Katacombs.Controllers.Responses;

/// <summary>
/// Represents a response containing location details.
/// </summary>
/// <param name="Description">The description of the location.</param>
/// <param name="Exits">The list of exits from the location.</param>
/// <param name="Items">The list of items in the location.</param>
public record LocationResponse(
    string Description,
    List<DirectionResponse> Exits,
    List<DirectionResponse> Items);

/// <summary>
/// Represents a response containing direction details.
/// </summary>
/// <param name="Direction">The direction value.</param>
public record DirectionResponse(string Direction);