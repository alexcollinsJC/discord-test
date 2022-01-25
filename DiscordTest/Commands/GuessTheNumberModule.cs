using Discord.Commands;
using DiscordTest.Games;
using JetBrains.Annotations;

namespace DiscordTest.Commands;

[Group("guessnumber")]
public class GuessTheNumberModule : ModuleBase<SocketCommandContext>
{
    private static GuessTheNumber game = new();


    [Command("start"), UsedImplicitly]
    public async Task StartAsync()
    {
        ulong player = Context.User.Id;
        game.StartGame(player, out string error);
        if (string.IsNullOrEmpty(error))
        {
            await ReplyAsync(game.GetGameState(player));
        }
        else
        {
            await ReplyAsync(error);
        }
    }

    [Command("guess"), UsedImplicitly]
    public async Task GuessAsync(int guess)
    {
        ulong player = Context.User.Id;
        bool gameFinished = game.TakeTurn(player, guess, out string output);
        if (gameFinished)
        {
            // In this case, game state is in the error string
            await ReplyAsync(output);
        }
        else if (!string.IsNullOrEmpty(output))
        {
            await ReplyAsync(output);
        }
        else
        {
            await ReplyAsync(game.GetGameState(player));
        }
    }

    [Command("last"), UsedImplicitly]
    public async Task LastAsync()
    {
        ulong player = Context.User.Id;
        await ReplyAsync(game.GetGameState(player));
    }
}
