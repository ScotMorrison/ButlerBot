using Discord.WebSocket;
using LeagueCustomMatchmaking.Matchmaking;

namespace ButlerBot;

internal class Preferences
{
    #region User Attributes
    public readonly SocketUser User;
    public readonly string Nickname;
    #endregion

    public int Top;
    public int Jungle;
    public int Mid;
    public int Adc;
    public int Support;

    public Preferences(SocketSlashCommand cmd)
    {
        User = cmd.User;
        Nickname = User.Username;
    }

    public Player ToPlayer()
    {
        return new(Nickname, 1000, new int[5] { Top, Jungle, Mid, Adc, Support });
    }
}