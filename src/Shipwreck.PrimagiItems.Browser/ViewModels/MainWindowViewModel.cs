namespace Shipwreck.PrimagiItems.Browser.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
    #region DocumentTitle

    private string? _DocumentTitle;

    public string? DocumentTitle
    {
        get => _DocumentTitle;
        set => SetProperty(ref _DocumentTitle, value);
    }

    #endregion DocumentTitle

    #region Location

    private const string LOGIN_URL = "https://primagi.jp/mypage/login/";

    private Uri _Location = new Uri("https://primagi.jp/mypage/");
    private bool _IsLogin;

    public Uri Location
    {
        get => _Location;
        set
        {
            if (SetProperty(ref _Location, value))
            {
                IsLogin = _Location?.ToString() == LOGIN_URL;
            }
        }
    }

    public bool IsLogin
    {
        get => _IsLogin;
        private set => SetProperty(ref _IsLogin, value);
    }

    #endregion Location
}