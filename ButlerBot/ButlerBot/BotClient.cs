using ButlerBot;
using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace LeagueCustomMatchmaking.BotClient;

public class BotClient
{
    private DiscordSocketClient _client;
    private string token = AppConfigReader.AccessToken;
    private LobbyView? _lobbyView;

    public async Task LoginAsync(bool writeCommands)
    {
        var config = new DiscordSocketConfig();
        _client = new DiscordSocketClient(config);
        
        _client.Log += Log;
        if (writeCommands) _client.Ready += WriteCommands;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.ButtonExecuted += ButtonHandler;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task WriteCommands()
    {
        var lobbyCommands = new SlashCommandBuilder()
            .WithName("lobby")
            .WithDescription("Lobby Commands.")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("create")
                .WithDescription("Create a new lobby if one is not currently running")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("cancel")
                .WithDescription("Cancel a lobby currently in progress")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("matchmake")
                .WithDescription("Run Matchmaking on a full lobby")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("start")
                .WithDescription("Start a full matchmade lobby")
                .WithType(ApplicationCommandOptionType.SubCommand));

        var prefCommand = new SlashCommandBuilder()
            .WithName("player-preferences")
            .WithDescription("Change your role preferences");

        List<ApplicationCommandProperties> scbList = new()
        {
            lobbyCommands.Build(), prefCommand.Build()
        };

        await _client.BulkOverwriteGlobalApplicationCommandsAsync(scbList.ToArray());
    }
    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "lobby":
                await HandleLobbyCommands(command);
                break;
            case "preferences":
                await HandlePreferencesCommand(command);
                break;
        }
    }
    private async Task ButtonHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "join-button":
                if (_lobbyView is not null) await _lobbyView.JoinLobby(component);
                else await component.RespondAsync("This lobby has expired.", ephemeral: true);
                break;
        }
    }
    private async Task HandleLobbyCommands(SocketSlashCommand command)
    {
        var subCommand = command.Data.Options.First().Name;

        switch (subCommand)
        {
            case "create":
                await CreateLobby(command);
                break;
            case "cancel":
                await CancelLobby(command);
                break;
            case "matchmake":
                await Matchmake(command);
                break;
            case "start":
                await StartGame(command);
                break;
        }
    }    
    private async Task CreateLobby(SocketSlashCommand command)
    {
        if (_lobbyView is null)
        {
            _lobbyView = new(command.Channel, command.User);
            await _lobbyView.LobbyCreated(command);
        }
        else
        {
            await command.RespondAsync("There appears to be a lobby already.", ephemeral: true);
        }
    }
    private async Task CancelLobby(SocketSlashCommand command)
    {
        if(_lobbyView is not null && Authorise(command.User))
        {
            await _lobbyView.DeleteLobby();
            _lobbyView = null;
            await command.RespondAsync("Lobby cancelled");
        }
        else
        {
            await command.RespondAsync("You are not hosting a lobby.", ephemeral: true);
        }
    }

    private bool Authorise(SocketUser user)
    {
        return user == _lobbyView.Host;
    }

    private async Task StartGame(SocketSlashCommand command)
    {
        await command.RespondAsync("Not yet implemented");
    }

    private async Task Matchmake(SocketSlashCommand command)
    {
        if (_lobbyView is not null && Authorise(command.User))
        {
            _lobbyView.Matchmake();
            await command.RespondAsync("Running matchmaking, be finished in a second boss.");
        }
        else
        {
            await command.RespondAsync("You are not hosting a lobby.", ephemeral: true);
        }
    }

    private async Task HandlePreferencesCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Not yet implemented");
    }
}