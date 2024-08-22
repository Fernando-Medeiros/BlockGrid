namespace MAUIGame;

public sealed partial class DevelopmentWindow : Window
{
    public DevelopmentWindow()
    {
        Width = 300;
        Height = 300;
        MinimumWidth = Width;
        MinimumHeight = Height;
        Page = new DevelopmentView(new DevelopmentViewModel());

        Dispatcher.DispatchAsync(async Task () =>
        {
            while (true)
            {
                Title = $"Heap: {GC.GetTotalMemory(true) / 1024 / 1024}MB";
                await Task.Delay(1500);
            }
        });
    }
}
