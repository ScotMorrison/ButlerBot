namespace LeagueCustomMatchmaking.Matchmaking;

public class Player
{
    public string Name;
    public int SkillValue;
    public int[] RolePreference;

    public Player(string name, int skill, int[] preferences)
    {
        Name = name;
        RolePreference = preferences;
        SkillValue = skill;
    }

    public int GetElo()
    {
        return SkillValue;
    }

    public int[] GetRolePreference()
    {
        return RolePreference;
    }

    public float GetRolePreference(int role)
    {
        return RolePreference[role];
    }

    public override string ToString()
    {
        return Name;
    }
}
