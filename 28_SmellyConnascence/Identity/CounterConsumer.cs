namespace SmellyConnascenceKata.Identity;

public class CounterConsumer
{
    public int RecordVisit()
    {
        return GlobalCounter.Instance.Increment();
    }

    public int TotalVisits()
    {
        return GlobalCounter.Instance.Current();
    }
}
