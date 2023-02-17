using Discord;
using Discord.WebSocket;

namespace LeagueCustomMatchmaking.Bot_Core;

public class BotCore
{
    private DiscordSocketClient _client;
    private const string token = "NTg4NzI5NDYxMzU3MTUwMjY5.GaAQei.LjmFssm7avH8dmhPB2xZmdqs1ZwxrvBnP2mNBA";
    private CommandHandler _handler;
    
    public async Task LoginAsync()
    {
        var config = new DiscordSocketConfig();
        _client = new DiscordSocketClient();

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _handler = new(_client, new());
        await _handler.InstallCommandsAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
