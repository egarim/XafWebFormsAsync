using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web;
using System;
using System.Threading.Tasks;
using System.Web.UI;

public class MyAsynController : ViewController
{
    public SimpleAction YourAsyncAction { get; private set; }

    public MyAsynController()
    {
        YourAsyncAction = new SimpleAction(this, "YourAsyncAction", "Category");
        YourAsyncAction.Execute += YourAsyncAction_Execute;
    }

    private void YourAsyncAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        // Create a progress reporter
        var progress = new Progress<int>(percent => 
        {
            // This action will be executed on the UI thread when progress is reported
            Application.ShowViewStrategy.ShowMessage($"Progress: {percent}%");
        });

        // Create and register the async task
        WebWindow.CurrentRequestPage.RegisterAsyncTask(
            new PageAsyncTask(async cancellationToken =>
            {
                try
                {
                    // Your async operation with progress reporting
                    var result = await YourAsyncMethodWithProgress(progress, cancellationToken);

                    // Update UI or process results
                    // Note: This runs after the async operation completes
                    Application.ShowViewStrategy.ShowMessage($"Operation completed: {result}");
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}");
                }
            })
        );
    }

    private async Task<string> YourAsyncMethodWithProgress(IProgress<int> progress, System.Threading.CancellationToken cancellationToken)
    {
        // Example async operation with progress reporting
        for (int i = 0; i <= 10; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                break;
                
            // Simulate work
            await Task.Delay(300, cancellationToken);
            
            // Report progress (0-100%)
            progress?.Report(i * 10);
        }
        
        return "Success";
    }
}