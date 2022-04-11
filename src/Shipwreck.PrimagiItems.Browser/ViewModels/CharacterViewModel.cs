namespace Shipwreck.PrimagiItems.Browser.ViewModels;

public sealed class CharacterViewModel : ObservableModel
{
    internal CharacterViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
    }

    public MainWindowViewModel MainWindow { get; }

    #region CardId

    private string? _CardId;

    public string? CardId
    {
        get => _CardId;
        set => SetProperty(ref _CardId, value);
    }

    #endregion CardId

    #region PlayerName

    private string? _PlayerName;

    public string? PlayerName
    {
        get => _PlayerName;
        set => SetProperty(ref _PlayerName, value);
    }

    #endregion PlayerName

    #region BirthMonth

    private byte _BirthMonth;

    public byte BirthMonth
    {
        get => _BirthMonth;
        set => SetProperty(ref _BirthMonth, value);
    }

    #endregion BirthMonth

    #region BirthDay

    private byte _BirthDay;

    public byte BirthDay
    {
        get => _BirthDay;
        set => SetProperty(ref _BirthDay, value);
    }

    #endregion BirthDay
}