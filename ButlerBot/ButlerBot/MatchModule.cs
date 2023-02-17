using Discord.Commands;

namespace LeagueCustomMatchmaking.IO;

[Group("match")]
public class MatchModule : ModuleBase<SocketCommandContext>
{
    [Command("create")]
    [Summary("Creates a new match.")]
    public async Task CreateMatch() => ReplyAsync("Match created, react :sam_emote: to join.");
}
