namespace SmellySolidKata.Dip;

// DIP violation: Kitchen (high-level policy) directly constructs a
// concrete MicrowaveOven (low-level detail) -- it can't work with any
// other kind of oven.
public class Kitchen
{
    private readonly MicrowaveOven _oven;

    public Kitchen()
    {
        _oven = new MicrowaveOven();
    }

    public void CookDinner()
    {
        _oven.Cook();
    }
}
