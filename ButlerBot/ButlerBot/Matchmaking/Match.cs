namespace LeagueCustomMatchmaking.Matchmaking;

public class Match
{
    public readonly Matchup Top;
    public readonly Matchup Jungle;
    public readonly Matchup Mid;
    public readonly Matchup Adc;
    public readonly Matchup Support;
    private Matchup[] _allMatchups;

    public Match(Matchup[] matchups)
    {
        Top = matchups[0];
        Jungle = matchups[1];
        Mid = matchups[2];
        Adc = matchups[3];
        Support = matchups[4];
        _allMatchups = matchups;
    }

    public void AutoBalance(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Balance();
        }
    }

    public int GetEloDifference()
    {
        return Top.EloDifference + Jungle.EloDifference + Mid.EloDifference + Adc.EloDifference + Support.EloDifference;
    }

    public override string ToString()
    {
        return $"{Top}\n{Jungle}\n{Mid}\n{Adc}\n{Support}";
    }

    private void Balance()
    {
        foreach (Matchup m in _allMatchups)
        {
            int eloSum = GetEloDifference();

            if (Math.Sign(m.EloDifference) == Math.Sign(eloSum)
                && m.EloDifference > 0
                && Math.Abs(eloSum) >= Math.Abs(m.EloDifference))
            {
                m.Swap();
            }
        }
    }
}