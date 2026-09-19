namespace SmellyConnascenceKata.Timing;

// Connascence of Timing: waiting a fixed, arbitrary delay instead of
// actually waiting on the job's completion -- correctness depends on the
// job finishing within 1000ms, a race condition disguised as a constant.
public class BackgroundJobRunner
{
    private string? _jobResult;

    public void StartJob()
    {
        Task.Delay(300).ContinueWith(_ => _jobResult = "done");
    }

    public async Task<string?> WaitForResult()
    {
        await Task.Delay(1000);
        return _jobResult;
    }
}
