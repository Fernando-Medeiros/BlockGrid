namespace GameUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        #region Views
        builder.Services.AddSingleton<MapView>();
        #endregion

        #region View Models
        builder.Services.AddSingleton<MapViewModel>();
        #endregion

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
