using Discord.WebSocket;

namespace DiscordTest.Games;

public class TicTacToe
{
    private DiscordSocketClient client;
    private Dictionary<ulong, TicTacToeInstance?> activeGames = new();

    public TicTacToe(DiscordSocketClient discordClient)
    {
        client = discordClient;
    }

    public void StartGame(ulong player1, string player2, ulong channel, out string error)
    {
        SocketUser? player2User = client.GetUser(player2, string.Empty);

        if (player2User == null)
        {
            error = "Invalid opponent specified.";
            return;
        }

        if (activeGames.ContainsKey(player1) || activeGames.ContainsKey(player2User.Id))
        {
            error = "Players are already in a game.";
            return;
        }

        TicTacToeInstance instance = new(player1, player2User.Id, channel);
        activeGames[player1] = activeGames[player2User.Id] = instance;

        error = string.Empty;
    }

    public ulong? TakeTurn(ulong player, ulong channel, int cell, out string error)
    {
        if (activeGames.TryGetValue(player, out TicTacToeInstance? instance) && instance != null)
        {
            if (instance.Channel != channel)
            {
                error = "Must play in the channel the game started in";
                return null;
            }

            instance.TakeTurn(player, cell, out error);
            if (instance.GameFinished)
            {
                foreach (ulong instancePlayer in instance.Players)
                {
                    activeGames.Remove(instancePlayer);
                }
            }

            return instance.Winner; // null if the game is not done
        }

        error = "Player is not in a game.";
        return null;
    }

    public string GetGameState(ulong player)
    {
        if (activeGames.TryGetValue(player, out TicTacToeInstance? instance) && instance != null)
        {
            return instance.GetGameState();
        }

        return string.Empty;
    }
}
