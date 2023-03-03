using Discord;

namespace ButlerBot;

public static class PreferenceHandler
{
    public static ComponentBuilder CreatePreferencesMenu()
    {
        return new ComponentBuilder()
            .WithSelectMenu(CreatePreferenceSelectMenu("TOP"))
            .WithSelectMenu(CreatePreferenceSelectMenu("JGL"))
            .WithSelectMenu(CreatePreferenceSelectMenu("MID"))
            .WithSelectMenu(CreatePreferenceSelectMenu("ADC"))
            .WithSelectMenu(CreatePreferenceSelectMenu("SUP"));
    }
    private static SelectMenuBuilder CreatePreferenceSelectMenu(string role)
    {
        var menuBuilder = new SelectMenuBuilder()
            .WithPlaceholder(role)
            .WithCustomId(role)
            .WithMinValues(1)
            .WithMaxValues(1);

        for(int i = 1; i <= 10; i++)
        {
            menuBuilder.AddOption($"{role}:{i}", $"{role}-{i}");
        }

        return menuBuilder;
    }

    public static ButtonBuilder CreateSubmitButton()
    {
        return new ButtonBuilder()
            .WithCustomId("pref-submit")
            .WithLabel("Submit")
            .WithStyle(ButtonStyle.Primary);
    }
}

public class PrefIO
{
    private const string _fileName = "PlayerPrefs.JSON";


    public void ReadPlayersFromFile()
    {
        throw new NotImplementedException();
    }

    public void WritePlayersToFile()
    {
        throw new NotImplementedException();
    }
}