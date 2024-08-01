namespace GameUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Dispatcher.DispatchAsync(CheckHeapMemoryUsage);
    }




    #region Development
    private async Task CheckHeapMemoryUsage()
    {
        int tiks = 0;
        while (true)
        {
            string heapMemory = $"Heap: {GC.GetTotalMemory(true) / 1024 / 1024}MB";
            Title = $"Tick: {tiks} :: {heapMemory}";

            await Task.Delay(1000);
            tiks++;
        }
    }
    #endregion
}
