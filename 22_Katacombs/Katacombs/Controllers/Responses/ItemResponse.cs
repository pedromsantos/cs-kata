namespace Katacombs.Controllers.Responses;

/// <summary>
/// Represents an item response.
/// </summary>
/// <param name="Sid">The SID of the item.</param>
/// <param name="Name">The name of the item.</param>
/// <param name="Description">The description of the item.</param>
/// <param name="Actions">The list of actions associated with the item.</param>
public record ItemResponse(
    string Sid,
    string Name,
    string Description,
    List<ActionResponse> Actions);

/// <summary>
/// Represents an action response.
/// </summary>
/// <param name="Action">The action associated with the response.</param>
public record ActionResponse(string Action);
