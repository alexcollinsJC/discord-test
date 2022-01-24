using Discord.Commands;

namespace DiscordTest.Commands
{
    public class EchoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            ReplyAsync(echo);
            return Task.CompletedTask;
        }
    }
}
