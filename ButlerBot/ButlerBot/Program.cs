using LeagueCustomMatchmaking.BotClient;

namespace ButlerBot;

internal class Program
{
    static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        Bot bot = new();

        await bot.LoginAsync(writeCommands: true);
    }
}