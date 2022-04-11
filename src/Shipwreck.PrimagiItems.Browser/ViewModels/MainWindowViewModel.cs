using Shipwreck.PrimagiItems.Browser.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Shipwreck.PrimagiItems.Browser.ViewModels;

public class MainWindowViewModel : WindowViewModel
{
    private static readonly string _DataPath
        = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), typeof(App).Namespace!, "data.json");

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

    #region Characters

    private BulkUpdateableCollection<CharacterViewModel>? _Characters;

    public BulkUpdateableCollection<CharacterViewModel> Characters
        => _Characters ??= new();

    private ReadOnlyCollection<CommandViewModelBase>? _CharacterCommands;
    private CommandViewModelBase? _AddCharacterCommand;

    public ReadOnlyCollection<CommandViewModelBase> CharacterCommands
        => _CharacterCommands ??= Array.AsReadOnly(new[] { AddCharacterCommand });

    public CommandViewModelBase AddCharacterCommand
        => _AddCharacterCommand
        ??= CommandViewModel.Create(
            () => { },
            title: "キャラクター追加", icon: "fas fa-plus");

    #endregion Characters

    private async void BeginLoad()
    {
        try
        {
            if (File.Exists(_DataPath))
            {
                BrowserDataContext d;
                using (var fs = new FileStream(_DataPath, FileMode.Open, FileAccess.Read))
                {
                    d = await Task.Run(() => Browser.Models.BrowserDataContext.Load(fs));
                }

                _Characters?.Clear();

                foreach (var c in d.Characters)
                {
                    Characters.Add(new CharacterViewModel(this)
                    {
                        CardId = c.CardId,
                        PlayerName = c.PlayerName,
                        BirthMonth = c.BirthMonth,
                        BirthDay = c.BirthDay,
                    });
                }
            }
        }
        catch { }
    }
}