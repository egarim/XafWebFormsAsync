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
        // Create and register the async task
        WebWindow.CurrentRequestPage.RegisterAsyncTask(
            new PageAsyncTask(async cancellationToken =>
            {
                try
                {
                    // Your async operation
                    var result = await YourAsyncMethod();

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

    private async Task<string> YourAsyncMethod()
    {
        // Example async operation
        await Task.Delay(3000); // Simulating work
        return "Success";
    }
}