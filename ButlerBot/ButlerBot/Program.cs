using LeagueCustomMatchmaking.Bot_Core;

namespace ButlerBot;

internal class Program
{
    static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        BotCore bot = new();

        await bot.LoginAsync();
    }
}