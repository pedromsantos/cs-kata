namespace Katacombs.Controllers.Responses;

/// <summary>
/// Represents a response containing a list of commands and hints.
/// </summary>
/// <param name="Commands">The list of commands.</param>
/// <param name="Hints">The list of hints.</param>
public record HelpResponse(
    List<Command> Commands,
    List<string> Hints);

/// <summary>
/// Represents a command with its name, description, and usage.
/// </summary>
/// <param name="Name">The name of the command.</param>
/// <param name="Description">The description of the command.</param>
/// <param name="Usage">The usage of the command.</param>
public record Command(string Name, string Description, string Usage);
