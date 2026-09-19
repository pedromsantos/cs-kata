namespace SmellySolidKata.Lsp;

// The `is Microwave` special-case here is the diagnostic signature of the
// LSP violation in Microwave: a caller that can't just trust Oven.Cook().
public class Chef
{
    public void Cook(Oven oven, string food)
    {
        if (oven is Microwave microwave)
        {
            microwave.CookMicrowaving(food);
        }
        else
        {
            oven.Cook(food);
        }
    }
}
