using Discord.Rest;
using Discord.WebSocket;
using Discord;

namespace ButlerBot;

public class Lobby
{
    private List<SocketUser> _players;
    public readonly SocketUser Host;
    public SocketMessageComponent JoinButton {get;set;}

    public Lobby(SocketUser host)
    {
        Host = host;
        _players = new();
    }

    public void Add(SocketUser user)
    {
        _players.Add(user);
    }

    public bool Contains(SocketUser user)
    {
        return _players.Contains(user);
    }

    public string Matchmake()
    {
        string output = "";
        foreach(SocketUser p in _players)
        {
            output += p.Mention;
        }
        return output;
    }
}
