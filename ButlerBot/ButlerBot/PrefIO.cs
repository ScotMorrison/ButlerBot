using Discord;
using LeagueCustomMatchmaking.Matchmaking;
using Newtonsoft.Json;

namespace ButlerBot;

public class PrefIO
{
    public Dictionary<string, Player> PlayersByMention;

    private string _fileName = "PlayerPrefs.JSON";

    public PrefIO()
    {
        PlayersByMention = ReadPlayersFromFile();
    }
    public Dictionary<string, Player> ReadPlayersFromFile()
    {
        Dictionary<string, Player> dict;
        try
        {
            string jsonString = File.ReadAllText(_fileName);
            dict = JsonConvert.DeserializeObject<Dictionary<string, Player>>(jsonString);
        }
        catch
        {
            dict = new();
        }
        
        return dict;
    }

    public void WritePlayersToFile()
    {
        string jsonString = JsonConvert.SerializeObject(PlayersByMention);
        File.WriteAllText(_fileName, jsonString);
    }
}