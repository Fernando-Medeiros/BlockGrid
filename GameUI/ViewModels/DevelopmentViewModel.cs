namespace GameUI.ViewModels;

public sealed partial class DevelopmentViewModel : BaseViewModel
{
    public DevelopmentViewModel()
    {
        TextureCommand = new Command(TextureChanged);
    }

    #region Property
    #endregion

    #region Command
    public ICommand TextureCommand { get; }
    #endregion

    #region Action
    private void TextureChanged(object sender)
    {
        if (IsBusy()) return;

        var texture = (TileTexture)Convert.ToByte(sender);
        App.Invoke(Event.TileTexture, texture);

        NotBusy();
    }
    #endregion
}
