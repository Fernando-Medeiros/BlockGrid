#if WINDOWS
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Input;

namespace MAUIGame.Common;

public sealed class BasePageHandler : PageHandler
{
    protected override void ConnectHandler(ContentPanel platformView)
    {
        base.ConnectHandler(platformView);
        platformView.PreviewKeyUp += OnPreviewKeyUp;
        platformView.PreviewKeyDown += OnPreviewKeyDown;
    }

    protected override void DisconnectHandler(ContentPanel platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.PreviewKeyUp -= OnPreviewKeyUp;
        platformView.PreviewKeyDown -= OnPreviewKeyDown;
    }

    private void OnPreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        App.Invoke(Event.KeyUp, Enum.GetName(e.Key));
    }

    private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        App.Invoke(Event.KeyDown, Enum.GetName(e.Key));
    }
}
#endif
