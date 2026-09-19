namespace SmellyConnascenceKata.Meaning;

// Connascence of Meaning: "1"/"2"/"3"/"4" only mean bike/car/train/bus by
// an unstated convention shared between the caller and this switch --
// nothing in the code documents or enforces the mapping.
public class TransportSelector
{
    private readonly List<string> _selected = new();

    public void SetTransport(string transport)
    {
        switch (transport)
        {
            case "1":
                _selected.Add("bike");
                break;
            case "2":
                _selected.Add("car");
                break;
            case "3":
                _selected.Add("train");
                break;
            case "4":
                _selected.Add("bus");
                break;
            default:
                throw new ArgumentException($"Unknown transport code: {transport}");
        }
    }
}
