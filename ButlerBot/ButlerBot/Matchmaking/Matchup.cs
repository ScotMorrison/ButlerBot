using System.Security.Principal;

namespace LeagueCustomMatchmaking.Matchmaking;

public class Matchup
{
    public Player Red { get; set; }
    public Player Blue { get; set; }
    public int EloDifference { get; set; }

    public readonly int Role;

    public Matchup(Player red, Player blue, int role)
    {
        Red = red;
        Blue = blue;
        Role = role;
        EloDifference = Blue.GetElo() - Red.GetElo();
    }

    public void Swap()
    {
        var temp = Red;
        Red = Blue;
        Blue = temp;
        EloDifference = -EloDifference;
    }

    public override string ToString()
    {
        return $"{Blue}:{Blue.GetRolePreference(Role)} vs {Red}:{Red.GetRolePreference(Role)} ";
    }
}
