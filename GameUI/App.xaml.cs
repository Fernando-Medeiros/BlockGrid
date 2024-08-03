namespace GameUI;

public sealed partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Dispatcher.DispatchAsync(() => ResourceContainer.LoadResourcesAsync());
        Dispatcher.DispatchAsync(() => OpenWindow(new DevelopmentWindow()));

        MainPage = new AppShell();
    }

    #region Property
    /// <summary>
    /// Pega a uníca instância do <see cref="GameEnvironment"/>
    /// Responsavel por manter as configurações em comum da aplicação.
    /// </summary>
    public static GameEnvironment GameEnvironment { get; } = new();

    /// <summary>
    /// Pega o uníca instância do <see cref="ResourceContainer"/>
    /// Contêiner com os recursos gráficos em comum da aplicação.
    /// </summary>
    public static ResourceContainer ResourceContainer { get; } = new();
    #endregion

    #region Event
    /// <summary>
    /// Subscreve uma rotina para ser executada quando o evento global for acionado.
    /// </summary>
    public static void Subscribe(Event @event, Action<object?> callback)
    {
        if (@event is Event.TileTexture)
            GameEnvironment.OnTileTexture += (sender, args) => callback(sender);

        if (@event is Event.LoadResource)
            ResourceContainer.OnLoadResource += (sender, args) => callback(sender);
    }

    /// <summary>
    /// Invoca um evento global passando argumentos.
    /// </summary>
    public static void Invoke(Event @event, object? args)
    {
        GameEnvironment.Invoke(@event, args);
        ResourceContainer.Invoke(@event, args);
    }
    #endregion
}
