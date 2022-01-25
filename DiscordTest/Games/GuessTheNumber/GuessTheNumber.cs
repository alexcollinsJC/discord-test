namespace DiscordTest.Games;

public class GuessTheNumber
{
    private Dictionary<ulong, GuessTheNumberInstance?> activeGames = new();

    public void StartGame(ulong player, out string error)
    {
        if (activeGames.ContainsKey(player))
        {
            error = "Player is already in a game.";
            return;
        }

        GuessTheNumberInstance instance = new();
        activeGames[player] = instance;

        error = string.Empty;
    }

    public bool TakeTurn(ulong player, int guess, out string output)
    {
        if (activeGames.TryGetValue(player, out GuessTheNumberInstance? instance) && instance != null)
        {
            bool gameFinished = instance.TakeTurn(guess, out output);
            if (gameFinished)
            {
                activeGames.Remove(player);
                output = instance.GetGameState();
            }

            return gameFinished;
        }

        output = "Player is not in a game.";
        return false;
    }

    public string GetGameState(ulong player)
    {
        if (activeGames.TryGetValue(player, out GuessTheNumberInstance? instance) && instance != null)
        {
            return instance.GetGameState();
        }

        return "Player is not in a game.";
    }
}
