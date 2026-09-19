namespace SmellySolidKata.Ocp;

// OCP violation: every new report format needs a new method on this
// controller (and a new concrete view class) -- the controller must be
// edited, not extended, to add a case.
public class CarEngineStatusReportController
{
    private readonly CarEngineViewModel _viewModel;

    public CarEngineStatusReportController(CarEngineViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public CarEngineWebView DisplayEngineStatusReport()
    {
        var webView = new CarEngineWebView();
        webView.FillWith(_viewModel);
        return webView;
    }

    public CarEnginePrintView PrintEngineStatusReport()
    {
        var printView = new CarEnginePrintView();
        printView.FillWith(_viewModel);
        return printView;
    }
}
