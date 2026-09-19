namespace SmellySolidKata.Ocp;

public class CarEngineWebView
{
    public string Html { get; private set; } = string.Empty;

    public void FillWith(CarEngineViewModel viewModel)
    {
        Html = $"<div>{viewModel.Rpm} rpm, {viewModel.Temperature}C</div>";
    }
}
