using ButlerBot;
using Discord.WebSocket;

namespace LeagueCustomMatchmaking.BotClient;

internal class PreferenceController
{
    private PreferenceView? _view;
    private Preferences? _prefs;
    private PrefIO _IO;

    public event EventHandler<CommandEventArgs> PreferenceRequest;
    public event EventHandler<CommandEventArgs> PreferencesExists;
    public event EventHandler<ComponentEventArgs> PreferencesSaved;

    public PreferenceController()
    {
        _IO = new();
    }

    public void HandleSlashCommand(SocketSlashCommand command)
    {
        CreatePreferences(command);
    }

    public void HandleSelect(SocketMessageComponent component)
    {
        string role = string.Join("", component.Data.Values).Split("-")[0];
        int preference = Int32.Parse(string.Join("", component.Data.Values).Split("-")[1]);
        switch (role)
        {
            case "TOP":
                _prefs.Top = preference;
                break;
            case "JGL":
                _prefs.Jungle = preference;
                break;
            case "MID":
                _prefs.Mid = preference;
                break;
            case "ADC":
                _prefs.Adc = preference;
                break;
            case "SUP":
                _prefs.Support = preference;
                break;
        }
    }

    public Task HandleButton(SocketMessageComponent component)
    {
        _IO.PlayersByMention[component.User.Mention] = _prefs.ToPlayer();
        _IO.WritePlayersToFile();
        PreferencesSaved?.Invoke(this, new(component));
        _view.DeleteForms();
        _view = null;
        _prefs = null;
        
        return Task.CompletedTask;
    } 

    public void CreatePreferences(SocketSlashCommand command)
    {
        if (_view is null && _prefs is null)
        {
            _view = new();
            _prefs = new(command);
            PreferenceRequest += _view.SendPreferencesMenu;
            PreferenceRequest?.Invoke(this, new(command));
        }
        else PreferencesExists?.Invoke(this, new(command));
    }
}