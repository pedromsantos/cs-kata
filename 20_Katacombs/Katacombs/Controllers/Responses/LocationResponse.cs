using Katacombs.Controllers.Requests;

namespace Katacombs.Controllers.Responses;

public record LocationResponse(
    string Description, 
    List<DirectionResponse> Exits, 
    List<DirectionResponse> Items);

public record DirectionResponse(string Direction);