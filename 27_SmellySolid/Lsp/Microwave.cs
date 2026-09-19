namespace SmellySolidKata.Lsp;

// LSP violation: Microwave can't honour Oven's Cook() contract, so it
// throws instead -- callers that only know about Oven get a broken promise.
public class Microwave : Oven
{
    public override void Cook(string food)
    {
        throw new NotSupportedException("Microwave does not support Cook(); use CookMicrowaving() instead");
    }

    public void CookMicrowaving(string food)
    {
        Console.WriteLine($"Microwaving {food}");
    }
}
