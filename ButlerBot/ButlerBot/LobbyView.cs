using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace ButlerBot;

public class LobbyView
{
    private Lobby _lobby;
    private EmbedBuilder? _embed;
    private RestUserMessage? _message;

    public LobbyView(Lobby lobby)
    {
        _lobby = lobby;
        _lobby.LobbyUpdated += Update;
        _lobby.LobbyCancelled += Delete;
    }
    public async Task Initialise()
    {
        _embed = new EmbedBuilder()
        {
            Title = "Players",
            Description = ""
        };

        var cb = new ComponentBuilder()
        .WithButton("Join", "lobby-join");
        _message = await _lobby.Channel.SendMessageAsync(embed: _embed.Build(), components: cb.Build());
    }

    private async void Update(object? sender, EventArgs args)
    {
        _embed.Description = "";
        foreach(SocketUser p in _lobby.Players)
        {
            _embed.Description += $"\n{p.Mention}";
        }
        await UpdateLobbyMessage();
    }

    private async void Delete(object? sender, EventArgs args)
    {
        await _message.DeleteAsync();
    }

    private async Task UpdateLobbyMessage()
    {
        await _message.ModifyAsync(x => x.Embed = _embed.Build());
    }

    
}
