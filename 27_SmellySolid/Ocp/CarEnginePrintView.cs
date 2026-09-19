namespace SmellySolidKata.Ocp;

public class CarEnginePrintView
{
    public string Text { get; private set; } = string.Empty;

    public void FillWith(CarEngineViewModel viewModel)
    {
        Text = $"RPM: {viewModel.Rpm}, Temp: {viewModel.Temperature}";
    }
}
