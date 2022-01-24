using Discord.Commands;
using JetBrains.Annotations;

namespace DiscordTest.Commands
{
    [UsedImplicitly]
    public class SayModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            ReplyAsync(echo);
            return Task.CompletedTask;
        }
    }
}
