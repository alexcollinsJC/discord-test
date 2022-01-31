using Discord;
using DiscordTest.Narrative;
using DiscordTest.Slash;

namespace DiscordTest.Commands;

public class StorySlashCommand : BaseSlashCommand
{
    override public string Name => "story";
    override protected string Description => "Story stuff";

    private class StartCommand : RunnableSlashCommand
    {
        override public string Name => "start";
        override protected string Description => "Start a story";
        override protected Delegate RunDelegate => StartStory;

        private void StartStory(string storyId)
        {
            var userId = Command.User.Id;
            var controller = NarrativeController.Instance;
            if (controller.StartNewNarrative(userId, storyId))
            {
                var content = controller.ProgressStory(userId);
                Command.RespondAsync(content);
            }
            else
            {
                Command.RespondAsync("oops");
            }
        }
    }

    private class ChooseCommand : RunnableSlashCommand
    {
        override public string Name => "choose";
        override protected string Description => "Make a choice";
        override protected Delegate RunDelegate => ChooseOption;

        private void ChooseOption(int index)
        {
            var userId = Command.User.Id;
            var controller = NarrativeController.Instance;
            if (controller.MakeChoice(userId, index))
            {
                var content = controller.ProgressStory(userId);
                Command.RespondAsync(content);
            }
            else
            {
                Command.RespondAsync("oops");
            }
        }
    }
}
