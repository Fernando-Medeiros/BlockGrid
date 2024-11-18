namespace SFMLGame.core.enums;

public enum EEvent : byte
{
    #region Binding
    BasicStatus,

    NodeChanged,
    SceneChanged,
    LoggerChanged,
    CameraChanged,
    SchemaChanged,
    EndGameChanged,
    SaveGameChanged,
    #endregion
    #region Window
    TextEntered,
    KeyPressed,
    KeyReleased,
    MouseMoved,
    MouseWheelScrolled,
    MouseButtonPressed,
    #endregion
}
