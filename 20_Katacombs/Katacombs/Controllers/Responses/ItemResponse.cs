namespace Katacombs.Controllers.Responses;

public record ItemResponse(
    string Sid, 
    string Name, 
    string Description,
    List<ActionResponse> Actions);

public record ActionResponse(string Action);
