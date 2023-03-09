
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace ButlerBot;

internal class PreferenceView
{
    private IUserMessage _form;
    private IUserMessage _submit;

    public async Task SendPreferencesMenu(SocketUser user)
    {
        ComponentBuilder prefMenuBuilder = PreferenceBuilder.CreatePreferencesMenu();

        var submitBuilder = new ComponentBuilder()
            .WithButton(PreferenceBuilder.CreateSubmitButton());

        _form = await user.SendMessageAsync("Preferences form: Rate each of your roles out of 5, where 5 is your main role.", components: prefMenuBuilder.Build());
        _submit = await user.SendMessageAsync("Submit when finished", components: submitBuilder.Build());
    }

    public async void DeleteForms()
    {
        await _form.DeleteAsync();
        await _submit.DeleteAsync();
    }
}
