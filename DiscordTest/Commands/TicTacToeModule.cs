using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordTest.Games;
using JetBrains.Annotations;

namespace DiscordTest.Commands;

[Group("tictactoe"), UsedImplicitly]
public class TicTacToeModule : ModuleBase<SocketCommandContext>
{
    private TicTacToe game;

    public TicTacToeModule(DiscordSocketClient discordClient)
    {
        game = new TicTacToe(discordClient);
    }

    [Command("start"), UsedImplicitly]
    public async Task StartAsync([Remainder] string opponent)
    {
        ulong player = Context.User.Id;
        game.StartGame(player, opponent, Context.Channel.Id, out string error);
        if (string.IsNullOrEmpty(error))
        {
            await ReplyAsync($"Game started against {opponent}!");
            await ReplyAsync(game.GetGameState(player));
        }
        else
        {
            await ReplyAsync(error);
        }
    }

    [Command("play"), UsedImplicitly]
    public async Task PlayAsync(int cell)
    {
        ulong player = Context.User.Id;
        ulong? winner = game.TakeTurn(player, Context.Channel.Id, cell, out string error);
        if (winner.HasValue)
        {
            IUser? user = await Context.Channel.GetUserAsync(winner.Value);
            await ReplyAsync($"{user?.Username} has won the game!");

            // In this case, game state is in the error string
            await ReplyAsync(error);
        }
        else if (!string.IsNullOrEmpty(error))
        {
            await ReplyAsync(error);
        }
        else
        {
            await ReplyAsync(game.GetGameState(player));
        }
    }
}
