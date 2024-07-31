namespace GameUI.Views;

public sealed partial class MapView : ContentPage
{
    public MapView(MapViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}