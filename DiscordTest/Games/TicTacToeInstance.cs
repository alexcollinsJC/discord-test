using System.Text;

namespace DiscordTest.Games;

public class TicTacToeInstance
{
    /*
     * 0 | 1 | 2
     * 3 | 4 | 5
     * 6 | 7 | 8
     */
    private const int gridSize = 9;

    private bool?[] grid = new bool?[gridSize];
    private bool turn = true;
    private bool? result = null;

    private ulong[] players = new ulong[2];

    public bool GameFinished => result.HasValue;
    public ulong? Winner => result.HasValue ? GetPlayer(result.Value) : null;

    public IEnumerable<ulong> Players => players;
    public ulong Channel { get; }

    public TicTacToeInstance(ulong player1, ulong player2, ulong channelId)
    {
        players[0] = player1;
        players[1] = player2;
        Channel = channelId;
    }

    private ulong GetPlayer(bool player) => player ? players[0] : players[1];

    public bool TakeTurn(ulong player, int cell, out string error)
    {
        if (GameFinished)
        {
            error = "The game has already finished.";
            return false;
        }

        ulong currentPlayer = GetPlayer(turn);
        if (currentPlayer != player)
        {
            error = "Not this player's turn.";
            return false;
        }

        int at = cell - 1; // Visuals are 1-indexed
        if (at is < 0 or >= gridSize || grid[at].HasValue)
        {
            error = "Invalid cell specified";
            return false;
        }

        grid[at] = turn;
        turn = !turn;
        CheckForWin();

        error = string.Empty;
        return result.HasValue;
    }

    private static readonly (int, int, int)[] wins = {
        (0, 1, 2),
        (3, 4, 5),
        (6, 7, 8),
        (0, 3, 6),
        (1, 4, 7),
        (2, 5, 8),
        (0, 4, 8),
        (3, 4, 6)
    };

    private void CheckForWin()
    {
        foreach ((int a, int b, int c) in wins)
        {
            bool? win = CheckForWin(a, b, c);
            if (win.HasValue)
            {
                result = win.Value;
                break;
            }
        }
    }

    private bool? CheckForWin(int a, int b, int c)
    {
        bool? aVal = grid[a];
        if (aVal.HasValue && aVal == grid[b] == grid[c])
        {
            return aVal.Value;
        }

        return null;
    }

    public string GetGameState()
    {
        StringBuilder sb = new();
        for (int y = 0 ; y < 3 ; y++)
        {
            for (int x = 0 ; x < 3 ; x++)
            {
                int at = y * 3 + x;
                bool? cell = grid[at];
                if (cell.HasValue)
                {
                    sb.Append(cell.Value ? "X" : "O");
                }
                else
                {
                    sb.Append($"{at + 1}");
                }

                if (x < 2)
                {
                    sb.AppendLine(" | ");
                }
            }

            if (y < 2)
            {
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}
