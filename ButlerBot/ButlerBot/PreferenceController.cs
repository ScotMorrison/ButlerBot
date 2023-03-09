using ButlerBot;
using Discord.WebSocket;

namespace LeagueCustomMatchmaking.BotClient;

internal class PreferenceController
{
    private Dictionary<ulong, PreferenceView> _views;
    private Dictionary<ulong, Preferences> _prefs;
    private PrefIO _IO;

    public event EventHandler<CommandEventArgs> PreferenceRequest;
    public event EventHandler<CommandEventArgs> PreferencesExists;
    public event EventHandler<ComponentEventArgs> PreferencesSaved;

    public PreferenceController(PrefIO prefs)
    {
        _IO = prefs;
        _views = new();
        _prefs = new();
    }

    public void HandleSlashCommand(SocketSlashCommand command)
    {
        CreatePreferences(command);
    }

    public void HandleSelect(SocketMessageComponent component)
    {
        string role = string.Join("", component.Data.Values).Split("-")[0];
        int preference = Int32.Parse(string.Join("", component.Data.Values).Split("-")[1]);
        var prefs = _prefs[component.User.Id];
        switch (role)
        {
            case "TOP":
                prefs.Top = preference;
                break;
            case "JGL":
                prefs.Jungle = preference;
                break;
            case "MID":
                prefs.Mid = preference;
                break;
            case "ADC":
                prefs.Adc = preference;
                break;
            case "SUP":
                prefs.Support = preference;
                break;
        }
    }

    public Task HandleButton(SocketMessageComponent component)
    {
        ulong userId = component.User.Id;
        _IO.PlayersByMention[component.User.Mention] = _prefs[userId].ToPlayer();
        _IO.WritePlayersToFile();
        PreferencesSaved?.Invoke(this, new(component));
        _views[userId].DeleteForms();
        _views.Remove(userId);
        _prefs.Remove(userId);
        
        return Task.CompletedTask;
    } 

    public void CreatePreferences(SocketSlashCommand command)
    {
        ulong userId = command.User.Id;
        if (!UserHasPreferenceMenu(userId))
        {
            _views[userId] = new();
            _prefs[userId] = new(command.User);
            _views[userId].SendPreferencesMenu(command.User);

            PreferenceRequest?.Invoke(this, new(command));
        }
        else PreferencesExists?.Invoke(this, new(command));
    }

    private bool UserHasPreferenceMenu(ulong userId)
    {
        return _views.ContainsKey(userId) && _prefs.ContainsKey(userId);
    }
}