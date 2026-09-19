namespace SmellySolidKata.Lsp;

public class GrillOven : Oven
{
    public override void Cook(string food)
    {
        Console.WriteLine($"Grilling {food}");
    }
}
