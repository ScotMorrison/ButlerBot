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
        lobby.LobbyCreated += LobbyCreated;
        lobby.LobbyCancelled += LobbyCancelled;
        lobby.JoinedLobby += JoinedLobby;
        lobby.MatchmakingRun += MatchmakingRun;
        lobby.LobbyStarted += LobbyStarted;
        lobby.LeftLobby += LeftLobby;

        lobby.NoActiveLobby += NoActiveLobby;
        lobby.UnauthorisedAccess += UnauthorisedAccess;
        lobby.InvalidCommand += InvalidCommand;
        lobby.LobbyNotExists += LobbyNotExists;
        lobby.LobbyExists += LobbyExists;
        lobby.WrongNumberOfPlayers += WrongNumberOfPlayers;
        lobby.AlreadyInLobby += AlreadyInLobby;
        lobby.LobbyFull += LobbyFull;
        lobby.NotInLobby += NotInLobby;
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
    private void JoinedLobby(object? sender, ComponentEventArgs e)
    {
        string response = "You have joined the lobby.";
        e.Component.RespondAsync(response, ephemeral: true);
    }
    private void MatchmakingRun(object? sender, CommandEventArgs e)
    {
        string response = "Running matchmaking on the current teams.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void LobbyStarted(object? sender, CommandEventArgs e)
    {
        string response = "Game has been started.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void LeftLobby(object? sender, ComponentEventArgs e)
    {
        string response = "You have left the lobby.";
        e.Component.RespondAsync(response, ephemeral: true);
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
    private void NoActiveLobby(object? sender, ComponentEventArgs e)
    {
        string response = "There is no active lobby to join.";
        e.Component.RespondAsync(response, ephemeral: true);
    }
    private void WrongNumberOfPlayers(object? sender, CommandEventArgs e)
    {
        string response = "The lobby does not have the right number of players to do this.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
    private void LobbyFull(object? sender, ComponentEventArgs e)
    {
        string response = "Sorry, the lobby is full.";
        e.Component.RespondAsync(response, ephemeral: true);
    }

    private void AlreadyInLobby(object? sender, ComponentEventArgs e)
    {
        string response = "You have already joined the lobby";
        e.Component.RespondAsync(response, ephemeral: true);
    }
    private void NotInLobby(object? sender, ComponentEventArgs e)
    {
        string response = "You are not in the lobby.";
        e.Component.RespondAsync(response, ephemeral: true);
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