using Discord.WebSocket;

namespace LeagueCustomMatchmaking.BotClient;

internal class PreferenceController
{
    private readonly DiscordSocketClient _client;
    public PreferenceController(DiscordSocketClient client)
    {
        _client = client;
    }
    public async Task Handle(SocketSlashCommand command)
    {
        throw new NotImplementedException();
        //await command.RespondAsync("Check your DMs for the preference menu", ephemeral: true);

        //ComponentBuilder prefMenuBuilder = PreferenceHandler.CreatePreferencesMenu();
        //var submitBuilder = new ComponentBuilder()
        //    .WithButton(PreferenceHandler.CreateSubmitButton());

        //await command.User.SendMessageAsync("Preferences form", components: prefMenuBuilder.Build());
        //await command.User.SendMessageAsync("Submit when finished", components: submitBuilder.Build());
    }
}