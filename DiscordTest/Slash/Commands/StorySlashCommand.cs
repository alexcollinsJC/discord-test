using Discord;
using Discord.WebSocket;
using DiscordTest.Narrative;
using DiscordTest.Slash;

namespace DiscordTest.Commands;

public class StorySlashCommand : BaseSlashCommand
{
    public override string Name => "story";
    protected override string Description => "Story stuff";

    private class StartCommand : RunnableSlashCommand
    {
        public override string Name => "start";
        protected override string Description => "Start a story";
        protected override Delegate RunDelegate => StartStory;

        private void StartStory(string storyId)
        {
            ulong userId = Command.User.Id;
            if (NarrativeController.Instance.StartNewNarrative(userId, storyId))
            {
                ProgressAndRespond(userId, Command, true);
            }
            else
            {
                Command.RespondAsync("oops");
            }
        }
    }

    private class ChooseCommand : RunnableSlashCommand
    {
        public override string Name => "choose";
        protected override string Description => "Make a choice";
        protected override Delegate RunDelegate => ChooseOption;

        private void ChooseOption(int index)
        {
            ulong userId = Command.User.Id;
            if (NarrativeController.Instance.MakeChoice(userId, index))
            {
                ProgressAndRespond(userId, Command, false);
            }
            else
            {
                Command.RespondAsync("oops");
            }
        }
    }

    private static void ProgressAndRespond(ulong userId, SocketSlashCommand command, bool start)
    {
        NarrativeController controller = NarrativeController.Instance;
        (string text, string[] choices) = controller.ProgressStory(userId);

        EmbedBuilder embed = new();
        if (start)
        {
            embed.WithTitle("The story begins...");
            embed.WithDescription(text);
        }
        else
        {
            string[] lines = text.Split('\n');
            embed.WithTitle(lines[0]);

            if (lines.Length > 1)
            {
                embed.WithDescription(string.Join('\n', lines, 1, lines.Length - 1));
            }
        }

        if (choices.Length > 0)
        {
            embed.WithThumbnailUrl("https://media2.littlezebra.com/159921-thickbox_default/monsieur-the-mouse-blue-fog.jpg");
        }
        else
        {
            embed.WithFooter("The story has ended");
        }

        for (int i = 0 ; i < choices.Length ; i++)
        {
            string choice = choices[i];
            embed.AddField($"{i})", choice);
        }

        command.RespondAsync(embeds: new[] { embed.Build() });
    }
}
