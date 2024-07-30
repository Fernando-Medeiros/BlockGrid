using System.ComponentModel;

namespace GameUI.Common;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    #region Properties
    private bool isBusy;
    #endregion

    #region UI Control
    protected void NotBusy() => isBusy = false;
    protected bool IsBusy() => isBusy || !(isBusy = true);
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    #endregion
}