using ButlerBot;
using Discord.WebSocket;

namespace LeagueCustomMatchmaking.BotClient;

internal class Responses
{
    public Responses(LobbyController lobbyController, PreferenceController prefController)
    {
        SubscribeToLobby(lobbyController);
        SubscribeToPreferences(prefController);
    }

    private void SubscribeToLobby(LobbyController lobby)
    {
        lobby.InvalidCommand += InvalidCommand;
        lobby.LobbyCreated += LobbyCreated;
        lobby.LobbyCancelled += LobbyCancelled;
        lobby.LobbyExists += LobbyExists;
        lobby.UnauthorisedAccess += UnauthorisedAccess;
    }

    private void SubscribeToPreferences(PreferenceController prefs)
    {
        prefs.PreferenceRequest += PreferenceRequest;
        prefs.PreferencesExists += PreferencesExists;
        prefs.PreferencesSaved += PreferencesSaved;
    }



    #region Success Messages
    #region Lobby Messages
    private void LobbyCreated(object? sender, CommandEventArgs e)
    {
        string response = "You have made a new lobby.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void LobbyCancelled(object? sender, CommandEventArgs e)
    {
        string response = "The lobby has been successfully cancelled.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    #endregion
    #region Preference Messages
    private void PreferenceRequest(object? sender, CommandEventArgs e)
    {
        string response = "Check your DMs for the preference menu";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void PreferencesSaved(object? sender, ComponentEventArgs e)
    {
        string response = "Preferences successfully saved";
        e.Component.RespondAsync(response, ephemeral: true);
    }
    #endregion
    #endregion

    #region Error Messages
    #region Lobby Messages
    private void InvalidCommand(object? sender, CommandEventArgs e)
    {
        string response = "This is an unrecognised or unimplemented command.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void UnauthorisedAccess(object? sender, CommandEventArgs e)
    {
        string response = "You are not allowed to perform this command as you are not the host of this lobby.";
        e.Command.RespondAsync(response, ephemeral: true);
    }

    private void LobbyExists(object? sender, CommandEventArgs e)
    {
        string response = "There is already an existing lobby.";
        e.Command.RespondAsync(response, ephemeral: true);
    }

    private void LobbyNotExists(object? sender, CommandEventArgs e)
    {
        string response = "There is currently no lobby.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    #endregion
    #region Preference Messages
    private async void PreferencesExists(object? sender, CommandEventArgs e)
    {
        string response = "You have already received the preferences menu";
        await e.Command.RespondAsync(response, ephemeral: true);
    }

    #endregion
    #endregion

}