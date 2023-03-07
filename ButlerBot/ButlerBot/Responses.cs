using ButlerBot;
using Discord.WebSocket;

namespace LeagueCustomMatchmaking.BotClient;

internal class Responses
{
    public Responses(LobbyController lobbyController, PreferenceController prefController)
    {
        SubscribeToLobby(lobbyController);
    }

    private void SubscribeToLobby(LobbyController lobby)
    {
        lobby.InvalidCommand += InvalidCommand;
        lobby.LobbyCreated += LobbyCreated;
        lobby.LobbyCancelled += LobbyCancelled;
        lobby.LobbyExists += LobbyExists;
        lobby.UnauthorisedAccess += UnauthorisedAccess;
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

    private void LobbyCancelled(object? sender, CommandEventArgs e)
    {
        string response = "The lobby has been successfully cancelled.";
        e.Command.RespondAsync(response, ephemeral: true);
    }

    private void LobbyCreated(object? sender, CommandEventArgs e)
    {
        string response = "You have made a new lobby.";
        e.Command.RespondAsync(response, ephemeral: true);
    }

    private void InvalidCommand(object? sender, CommandEventArgs e)
    {
        string response = "This is an unrecognised or unimplemented command.";
        e.Command.RespondAsync(response, ephemeral: true);
    }
}