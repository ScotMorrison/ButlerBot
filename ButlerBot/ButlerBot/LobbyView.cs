using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.ComponentModel;
using System.Threading.Channels;

namespace ButlerBot;

public class LobbyView
{
    private EmbedBuilder _embedBuilder;
    private RestUserMessage _matchMessage;
    private ISocketMessageChannel _channel;
    private Lobby _lobby;
    public SocketUser Host;

    public LobbyView(ISocketMessageChannel channel, SocketUser host)
    {
        Host = host;
        _channel = channel;
        _lobby = new(host);

        _embedBuilder = new EmbedBuilder()
        {
            Title = "Players",
            Description = ""
        };
        AddUserToLobby(host);
    }

    private async Task UpdateMatchMessage()
    {
        await _matchMessage.ModifyAsync(x => x.Embed = _embedBuilder.Build());
    }

    public async Task LobbyCreated(SocketSlashCommand command)
    {
        var cb = new ComponentBuilder()
        .WithButton("Join", "join-button");

        await command.RespondAsync("Match Created");
        _matchMessage = await _channel.SendMessageAsync(embed: _embedBuilder.Build(), components: cb.Build());
    }

    public async Task JoinLobby(SocketMessageComponent component)
    {
        string response;
        if (!_lobby.Contains(component.User))
        {
            AddUserToLobby(component.User);
            await UpdateMatchMessage();
            response = "Joined match";
        }
        else
        {
            response = "You are already in the match";
        }
        await component.RespondAsync(response);
    }

    private void AddUserToLobby(SocketUser user)
    { 
        _lobby.Add(user);
        _embedBuilder.Description += $"\n{user.Mention}";
    }

    public async Task DeleteLobby()
    {
        await _matchMessage.DeleteAsync();
    }

    internal void Matchmake()
    {
        throw new NotImplementedException();
    }
}
