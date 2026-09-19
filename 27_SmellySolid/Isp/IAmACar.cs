namespace SmellySolidKata.Isp;

public record Location(double Lat, double Lng);

// ISP violation: gasoline and electric refuelling are mutually exclusive
// capabilities bundled into one fat interface -- no single car honestly
// supports both.
public interface IAmACar
{
    void GoTo(Location location);
    void RefillGasoline(int gallons);
    void RefillElectricity(decimal kiloWatts);
    int CurrentMileage();
}
