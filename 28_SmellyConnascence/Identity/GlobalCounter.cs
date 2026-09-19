namespace SmellyConnascenceKata.Identity;

// Connascence of Identity: correctness of every consumer depends on
// them all sharing this exact single static instance -- there is no way
// to have two independent counters, and the dependency is invisible from
// any one consumer's own code.
public class GlobalCounter
{
    public static readonly GlobalCounter Instance = new();

    private int _value;

    public int Increment()
    {
        return ++_value;
    }

    public int Current()
    {
        return _value;
    }
}
