
using Discord;
using Discord.Rest;
namespace ButlerBot;

internal class PreferenceView
{
    private IUserMessage _form;
    private IUserMessage _submit;

    public async void SendPreferencesMenu(object? sender, CommandEventArgs e)
    {
        ComponentBuilder prefMenuBuilder = PreferenceBuilder.CreatePreferencesMenu();

        var submitBuilder = new ComponentBuilder()
            .WithButton(PreferenceBuilder.CreateSubmitButton());

        _form = await e.Command.User.SendMessageAsync("Preferences form", components: prefMenuBuilder.Build());
        _submit = await e.Command.User.SendMessageAsync("Submit when finished", components: submitBuilder.Build());
    }

    public async void DeleteForms()
    {
        await _form.DeleteAsync();
        await _submit.DeleteAsync();
    }
}
