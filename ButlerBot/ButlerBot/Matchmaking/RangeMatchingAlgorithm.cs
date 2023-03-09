namespace LeagueCustomMatchmaking.Matchmaking;

public class RangeMatchingAlgorithm : MatchingAlgorithm
{
    private readonly int _minimumPreference;
    private List<Player>[] _playerListsByRole;

    public RangeMatchingAlgorithm(List<Player> players, int minimumPreference) : base(players)
    {
        _minimumPreference = minimumPreference;
    }

    public override Match CreateMatch()
    {
        Matchup[] matchups = new Matchup[5];
        CreateRanges(_minimumPreference);
        while (_matchupOrder.Count > 0)
        {
            int role = FindSmallestRange();
            matchups[role] = ChooseMatchupFromRange(role);
        }

        return new(matchups);
    }

    private void CreateRanges(int minimumPreference)
    {
        List<Player>[] output = new List<Player>[5];

        for (int i = 0; i < output.Length; i++)
        {
            int role = i;
            List<Player> currentList = output[role];

            output[i] = _workingListOfPlayers.Where((p) => p.GetRolePreference(role) >= minimumPreference).ToList();

            if (!output[i].Any())
            {
                throw new Exception($"No players meet the preference requirement for role {role}");
            }
        }

        _playerListsByRole = output;
    }

    private int FindSmallestRange()
    {
        int output = _matchupOrder[0];
        int smallest = _playerListsByRole[output].Count;

        foreach (int role in _matchupOrder)
        {
            if (_playerListsByRole[role].Count < smallest)
            {
                smallest = _playerListsByRole[role].Count;
                output = role;
            }
        }

        return output;
    }

    private Matchup ChooseMatchupFromRange(int role)
    {
        List<Player> range = _playerListsByRole[role];
        range = Shuffle(range);

        if (range.Count < 2)
        {
            throw new Exception($"Not enough remaining players for role {role}");
        }

        Matchup output = new(range[0], range[1], role);
        _matchupOrder.Remove(role);
        foreach (List<Player> playerRange in _playerListsByRole)
        {
            playerRange.Remove(output.Blue);
            playerRange.Remove(output.Red);
        }

        _workingListOfPlayers.Remove(output.Blue);
        _workingListOfPlayers.Remove(output.Red);

        return output;
    }
}
