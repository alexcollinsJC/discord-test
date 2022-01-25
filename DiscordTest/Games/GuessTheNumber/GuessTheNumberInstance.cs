namespace DiscordTest.Games;

public class GuessTheNumberInstance
{
    private int number;
    private List<int> guesses = new();

    private static readonly Random rand = new();

    private const int MaxValue = 100;
    private const int MaxGuesses = 10;

    private bool GameFinished => guesses[^1] == number || guesses.Count == MaxGuesses;

    public GuessTheNumberInstance()
    {
        number = rand.Next(MaxValue + 1);
    }

    public bool TakeTurn(int guess, out string error)
    {
        if (guess is < 0 or > MaxValue)
        {
            error = "Guess is outside of the range of 0 --> 100";
            return false;
        }

        error = string.Empty;
        guesses.Add(guess);

        return GameFinished;
    }

    public string GetGameState()
    {
        if (guesses.Count == 0)
        {
            return $"Guess the random number between 0 and {MaxValue} in {MaxGuesses} guesses!";
        }

        int lastGuess = guesses[^1];
        if (lastGuess == number)
        {
            return $"Guessed number '**{number}**' correctly in {guesses.Count} {GetGuesses(guesses.Count)}!";
        }

        if (guesses.Count == MaxGuesses)
        {
            return $"Ran out of guesses! Number was {number}. Guesses : {string.Join(", ", guesses)}.";
        }

        string closeStr = $"{(Math.Abs(lastGuess - number) < 5 ? "**Close!** " : string.Empty)}";
        int left = MaxGuesses - guesses.Count;
        string guessesLeftStr = $"{left} {GetGuesses(left)} left.";

        return $"{closeStr}Guess of '{lastGuess}' was too {(lastGuess < number ? "low" : "high")}. {guessesLeftStr}";
    }

    private static string GetGuesses(int guessCount) => $"guess{(guessCount == 1 ? string.Empty : "es")}";
}
