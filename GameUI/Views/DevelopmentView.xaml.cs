namespace GameUI.Views;

public sealed partial class DevelopmentView : ContentPage
{
    public DevelopmentView(DevelopmentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}