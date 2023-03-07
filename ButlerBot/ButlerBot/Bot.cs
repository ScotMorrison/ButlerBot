using ButlerBot;
using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace LeagueCustomMatchmaking.BotClient;

public class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly string _token;

    private LobbyController _lobbyController;
    private PreferenceController _prefController;
    private Responses _responses;

    private const string _lobbyCommand = "lobby";
    private const string _preferencesCommand = "preferences";

    public Bot()
    {
        _token = AppConfigReader.AccessToken;

        var config = new DiscordSocketConfig();
        _client = new DiscordSocketClient(config);

        _lobbyController = new();
        _prefController = new(_client);
        _responses = new(_lobbyController, _prefController);
    }

    public async Task LoginAsync(bool writeCommands)
    {
        //setup event handlers
        _client.Log += Log;
        if (writeCommands) _client.Ready += CreateSlashCommands;
        _client.SlashCommandExecuted += HandleSlashCommand;
        _client.ButtonExecuted += HandleButton;
        _client.SelectMenuExecuted += HandleSelect;

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    private async Task HandleSlashCommand(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case _lobbyCommand:
                await _lobbyController.Handle(command);
                break;
            case _preferencesCommand:
                await _prefController.Handle(command);
                break;
        }
    }
    private async Task HandleSelect(SocketMessageComponent arg)
    {
        throw new NotImplementedException();
        //var text = string.Join(", ", arg.Data.Values);
        //await arg.RespondAsync($"You have selected {text}");
    }
    private Task HandleButton(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task CreateSlashCommands()
    {
        var lobbyCommands = new SlashCommandBuilder()
            .WithName(_lobbyCommand)
            .WithDescription("Lobby Commands.");

        lobbyCommands = _lobbyController.AddCommands(lobbyCommands);
        
        var prefCommand = new SlashCommandBuilder()
            .WithName(_preferencesCommand)
            .WithDescription("Change your role preferences");

        List<ApplicationCommandProperties> scbList = new()
        {
            lobbyCommands.Build(), prefCommand.Build()
        };

        await _client.BulkOverwriteGlobalApplicationCommandsAsync(scbList.ToArray());
    }
}