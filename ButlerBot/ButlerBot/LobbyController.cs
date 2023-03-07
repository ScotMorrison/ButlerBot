using Discord;
using Discord.WebSocket;
using System.Security.Policy;

namespace ButlerBot;

public class LobbyController
{
    private Lobby? _lobby;
    private LobbyView? _view;

    #region Command names
    private const string _create = "create";
    private const string _matchmake = "matchmake";
    private const string _start = "start";
    private const string _cancel = "cancel";
    #endregion

    #region Events
    public event EventHandler<CommandEventArgs> LobbyCreated;
    public event EventHandler<CommandEventArgs> LobbyCancelled;
    public event EventHandler<CommandEventArgs> InvalidCommand;
    public event EventHandler<CommandEventArgs> LobbyExists;
    public event EventHandler<CommandEventArgs> UnauthorisedAccess;
    #endregion

    public async Task Handle(SocketSlashCommand command)
    {
        var subCommand = command.Data.Options.First().Name;
        switch (subCommand)
        {
            case _create:
                await CreateLobby(command);
                LobbyCreated.Invoke(this, new(command));
                break;
            case _matchmake:
                await Matchmake(command);
                break;
            case _cancel:
                await CancelLobby(command);
                break;
            case _start:
                await StartLobby(command);
                break;
            default:
                //Send unrecognised message
                break;
        }
    }
    public async Task CreateLobby(SocketSlashCommand command)
    {
        if(_lobby is null)
        {
            _lobby = new(command.User, command.Channel);
            _view = new(_lobby);
            await _view.Initialise();
            _lobby.Add(command.User);
        }
    }
    private Task StartLobby(SocketSlashCommand command)
    {
        throw new NotImplementedException();
    }

    private async Task CancelLobby(SocketSlashCommand command)
    {
        if (_lobby.Authorise(command.User))
        {
            _lobby.Cancel();
            _lobby = null;
            _view = null;
        }
        else
        {
            UnauthorisedAccess.Invoke(this, new(command));
        }
    }

    private Task Matchmake(SocketSlashCommand command)
    {
        throw new NotImplementedException();
    }


    public SlashCommandBuilder AddCommands(SlashCommandBuilder scb)
    {
        return scb.AddOption(new SlashCommandOptionBuilder()
                .WithName(_create)
                .WithDescription("Create a new lobby if one is not currently running")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName(_cancel)
                .WithDescription("Cancel a lobby currently in progress")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName(_start)
                .WithDescription("Run Matchmaking on a full lobby")
                .WithType(ApplicationCommandOptionType.SubCommand))
            .AddOption(new SlashCommandOptionBuilder()
                .WithName(_matchmake)
                .WithDescription("Start a full matchmade lobby")
                .WithType(ApplicationCommandOptionType.SubCommand));
    }  
}

public class CommandEventArgs
{
    public SocketSlashCommand Command;

    public CommandEventArgs(SocketSlashCommand cmd)
    {
        Command = cmd;
    }
}


