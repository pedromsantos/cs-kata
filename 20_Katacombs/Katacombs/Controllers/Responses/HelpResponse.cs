namespace Katacombs.Controllers.Responses;

public record HelpResponse(
    List<Command> Commands,
    List<string> Hints);

public record Command(string Name, string Description, string Usage);
