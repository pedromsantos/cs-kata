namespace SmellySolidKata.Isp;

public class ElectricCar : IAmACar
{
    private int _mileage;
    private decimal _batteryKiloWatts;

    public void GoTo(Location location)
    {
        _mileage += 1;
        Console.WriteLine($"Driving to {location.Lat}, {location.Lng}");
    }

    public void RefillGasoline(int gallons)
    {
        throw new NotSupportedException("Electric cars don't take gasoline");
    }

    public void RefillElectricity(decimal kiloWatts)
    {
        _batteryKiloWatts += kiloWatts;
    }

    public int CurrentMileage()
    {
        return _mileage;
    }
}
