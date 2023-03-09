using Discord.WebSocket;
using LeagueCustomMatchmaking.Matchmaking;

namespace ButlerBot;

internal class Preferences
{
    public readonly string Nickname;
    public readonly ulong UserId;

    public int Top;
    public int Jungle;
    public int Mid;
    public int Adc;
    public int Support;

    public Preferences(SocketUser user)
    {
        Nickname = user.Username;
        UserId = user.Id;
    }

    public Player ToPlayer()
    {
        return new(Nickname, 1000, new int[5] { Top, Jungle, Mid, Adc, Support });
    }
}