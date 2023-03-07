namespace LeagueCustomMatchmaking.Matchmaking;

public abstract class MatchingAlgorithm
{
    protected List<Player> _workingListOfPlayers;
    protected readonly List<Player> _initialPlayers;
    protected readonly List<int> _initialRoles;
    protected List<int> _matchupOrder;

    public MatchingAlgorithm(List<Player> players)
    {
        _initialPlayers = players;
        _initialRoles = new List<int> { 0, 1, 2, 3, 4 };
        Init();
    }

    public abstract Match CreateMatch();

    protected List<T> Shuffle<T>(List<T> list)
    {
        Random rnd = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

        return list;
    }

    protected void Init()
    {
        _workingListOfPlayers = _initialPlayers;
        _matchupOrder = _initialRoles;
    }
}