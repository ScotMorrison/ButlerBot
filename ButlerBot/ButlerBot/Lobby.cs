using Discord.Rest;
using Discord.WebSocket;
using Discord;

namespace ButlerBot;

public class Lobby
{
    public readonly SocketUser Host;
    public readonly ISocketMessageChannel Channel;
    public readonly List<SocketUser> Players;

    #region Events
    public event EventHandler<EventArgs> LobbyUpdated;
    public event EventHandler<EventArgs> LobbyStarted;
    public event EventHandler<EventArgs> LobbyCancelled;
    #endregion

    public Lobby(SocketUser host, ISocketMessageChannel channel)
    {
        Host = host;
        Channel = channel;
        Players = new();
    }

    public void Add(SocketUser user)
    {
        Players.Add(user);
        LobbyUpdated?.Invoke(this, new());
    }

    public bool Contains(SocketUser user) => Players.Contains(user);
    public bool Authorise(SocketUser user) => user == Host;
    public void Cancel() => LobbyCancelled.Invoke(this, new());

    public void Matchmake()
    {
        LobbyUpdated.Invoke(this, new());
    }
}