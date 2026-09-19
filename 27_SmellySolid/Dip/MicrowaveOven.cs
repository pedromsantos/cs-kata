namespace SmellySolidKata.Dip;

// DIP violation: MicrowaveOven news up a concrete MicrowaveGenerator
// itself instead of depending on an injected abstraction.
public class MicrowaveOven
{
    private readonly MicrowaveGenerator _heater;

    public MicrowaveOven()
    {
        _heater = new MicrowaveGenerator();
    }

    public void Cook()
    {
        _heater.Generate();
    }
}
