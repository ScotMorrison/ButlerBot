namespace LeagueCustomMatchmaking.Matchmaking;

public class RandomMatchingAlgorithm : MatchingAlgorithm
{
    public RandomMatchingAlgorithm(List<Player> players) : base(players)
    {

    }

    public override Match CreateMatch()
    {
        Matchup[] matchups = new Matchup[5];

        Matchup randomMatchup = CreateRandomMatchup();

        matchups[randomMatchup.Role] = randomMatchup;

        while (_workingListOfPlayers.Any())
        {
            Matchup m = CreatePreferredMatchup();
            matchups[m.Role] = m;
        }

        return new(matchups);
    }

    private Matchup CreateRandomMatchup()
    {
        _matchupOrder = Shuffle(_matchupOrder);
        _workingListOfPlayers = Shuffle(_workingListOfPlayers);
        int role = _matchupOrder[0];

        Matchup output = new(_workingListOfPlayers[0], _workingListOfPlayers[1], role);

        _workingListOfPlayers.Remove(output.Red);
        _workingListOfPlayers.Remove(output.Blue);
        _matchupOrder.Remove(role);

        return output;
    }

    private Matchup CreatePreferredMatchup()
    {
        int role = _matchupOrder[0];
        Player[] top = new Player[2] { _workingListOfPlayers[0], _workingListOfPlayers[1] };

        for (int i = 2; i < _workingListOfPlayers.Count; i++)
        {
            if (_workingListOfPlayers[i].GetRolePreference(role) > top[0].GetRolePreference(role))
            {
                if (_workingListOfPlayers[i].GetRolePreference(role) > top[1].GetRolePreference(role))
                {
                    top[0] = top[1];
                    top[1] = _workingListOfPlayers[i];
                }
                else
                {
                    top[0] = _workingListOfPlayers[i];
                }
            }
        }

        Matchup output = new(top[0], top[1], role);
        _workingListOfPlayers.Remove(output.Blue);
        _workingListOfPlayers.Remove(output.Red);
        _matchupOrder.Remove(role);

        return output;
    }
}
