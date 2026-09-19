using System.Text.Json;

namespace SmellySolidKata.Srp;

public record Location(double Lat, double Lng);

// SRP violation: Car mixes domain behaviour (mileage/travel) with a
// persistence concern (save) -- two different reasons to change bundled
// into one class.
public class Car
{
    private int _mileage;
    private Location _location = new(0, 0);

    public int CurrentMileage()
    {
        return _mileage;
    }

    public void TravelTo(Location location)
    {
        _location = location;
        _mileage += 1;
    }

    public void Save()
    {
        var row = JsonSerializer.Serialize(new { Mileage = _mileage, Location = _location });
        File.WriteAllText("/tmp/car.json", row);
    }
}
