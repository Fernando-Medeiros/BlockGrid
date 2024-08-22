namespace MAUIGame.Views;

public sealed partial class MapView : ContentPage
{
    public MapView(MapViewModel vm)
    {
        InitializeComponent();

        vm._scrollView = _scrollView;

        BindingContext = vm;
    }
}