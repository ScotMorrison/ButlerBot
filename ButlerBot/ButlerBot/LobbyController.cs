using Discord;
using Discord.WebSocket;
using LeagueCustomMatchmaking.Matchmaking;
using System.ComponentModel;
using System.Security.Policy;

namespace ButlerBot;

public class LobbyController
{
    private Lobby? _lobby;
    private LobbyView? _view;
    private PrefIO _prefs;

    #region Command names
    private const string _create = "create";
    private const string _matchmake = "matchmake";
    private const string _start = "start";
    private const string _cancel = "cancel";
    #endregion

    #region Events
    public event EventHandler<CommandEventArgs> LobbyCreated;
    public event EventHandler<CommandEventArgs> LobbyCancelled;
    public event EventHandler<CommandEventArgs> MatchmakingRun;
    public event EventHandler<CommandEventArgs> LobbyStarted;
    public event EventHandler<ComponentEventArgs> JoinedLobby;
    public event EventHandler<ComponentEventArgs> LeftLobby;

    public event EventHandler<CommandEventArgs> InvalidCommand;
    public event EventHandler<CommandEventArgs> LobbyExists;
    public event EventHandler<CommandEventArgs> LobbyNotExists;
    public event EventHandler<CommandEventArgs> UnauthorisedAccess;
    public event EventHandler<CommandEventArgs> WrongNumberOfPlayers;
    public event EventHandler<ComponentEventArgs> NoActiveLobby;
    public event EventHandler<ComponentEventArgs> AlreadyInLobby;
    public event EventHandler<ComponentEventArgs> LobbyFull;
    public event EventHandler<ComponentEventArgs> NotInLobby;

    #endregion

    public LobbyController(PrefIO prefs)
    {
        _prefs = prefs;
    }

    public async Task HandleSlashCommand(SocketSlashCommand command)
    {
        var subCommand = command.Data.Options.First().Name;
        switch (subCommand)
        {
            case _create:
                await CreateLobby(command);
                LobbyCreated?.Invoke(this, new(command));
                break;
            case _matchmake:
                Matchmake(command);
                MatchmakingRun?.Invoke(this, new(command));
                break;
            case _cancel:
                CancelLobby(command);
                LobbyCancelled?.Invoke(this, new(command));
                break;
            case _start:
                StartLobby(command);
                LobbyStarted?.Invoke(this, new(command));
                break;
            default:
                InvalidCommand?.Invoke(this, new(command));
                break;
        }
    }

    public Task HandleButton(SocketMessageComponent component)
    {
        var id = component.Data.CustomId.Split("-")[1];

        switch (id)
        {
            case "join":
                JoinLobby(component);
                break;
            case "leave":
                LeaveLobby(component);
                break;
            default:
                break;
        }
        return Task.CompletedTask;
    }

    private void LeaveLobby(SocketMessageComponent component)
    {
        SocketUser user = component.User;
        if (!_lobby.Contains(user))
        {
            NotInLobby?.Invoke(this, new(component));
        }
        else
        {
            _lobby.Remove(user);
            LeftLobby?.Invoke(this, new(component));
        }
    }

    private async Task CreateLobby(SocketSlashCommand command)
    {
        if(_lobby is null)
        {
            _lobby = new(command.User, command.Channel);
            _view = new(_lobby);
            await _view.Initialise();
            _lobby.Add(command.User);
        }
        else
        {
            LobbyExists?.Invoke(this, new(command));
        }
    }
 
    private void CancelLobby(SocketSlashCommand command)
    {
        if (CheckLobbyExistsAndUserIsHost(command))
        {
            _lobby.Cancel();
            _lobby = null;
            _view = null;
            LobbyCancelled?.Invoke(this, new(command));
        }
    }

    private async void Matchmake(SocketSlashCommand command)
    {
        if (CheckLobbyExistsAndUserIsHost(command))
        {
            List<Player> playerList = new();
            foreach(SocketUser p in _lobby.Players)
            {
                try
                {
                    Player player = _prefs.PlayersByMention[p.Mention];
                    playerList.Add(player);
                }
                catch
                {
                    Console.WriteLine($"{p.Username} has no preferences set");
                }
            }

            if(playerList.Count != 10)
            {
                WrongNumberOfPlayers?.Invoke(this, new(command));
            }
            else
            {
                MatchmakingRun?.Invoke(this, new(command));
                var rma = new RangeMatchingAlgorithm(playerList, 3);
                Match match = rma.CreateMatch();
                await _view.PostMatchmadeGame(match);
            }
        }
    }
    private void StartLobby(SocketSlashCommand command)
    {
        if (CheckLobbyExistsAndUserIsHost(command))
        {
            throw new NotImplementedException();
        }
    }

    private void JoinLobby(SocketMessageComponent component)
    {
        if(_lobby is null)
        {
            NoActiveLobby?.Invoke(this, new(component));

        }
        else if(_lobby.Contains(component.User))
        {
            AlreadyInLobby?.Invoke(this, new(component));
        }
        else if(_lobby.Count() == 10)
        {
            LobbyFull?.Invoke(this, new(component));
        }
        else
        {
            _lobby.Add(component.User);
            JoinedLobby?.Invoke(this, new(component));
        }

    }
    private bool CheckLobbyExistsAndUserIsHost(SocketSlashCommand command)
    {
        if (_lobby is null)
        {
            LobbyNotExists?.Invoke(this, new(command));
            return false;
        }
        else if (!_lobby.Authorise(command.User))
        {
            UnauthorisedAccess?.Invoke(this, new(command));
            return false;
        }
        else return true;
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

public class ComponentEventArgs
{
    public SocketMessageComponent Component;

    public ComponentEventArgs(SocketMessageComponent cmp)
    {
        Component = cmp;
    }
}
