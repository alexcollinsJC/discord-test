using DiscordTest.Narrative;
using DiscordTest.Slash;

namespace DiscordTest.Commands;

public class StorySlashCommand : BaseSlashCommand
{
    override public string Name => "story";
    override protected string Description => "Story stuff";
    override protected IEnumerable<BaseSlashCommand> SubCommands => 
        new BaseSlashCommand[] { new StartCommand(), new ChooseCommand() };


    private class StartCommand : RunnableSlashCommand
    {
        override public string Name => "start";
        override protected string Description => "Start a story";
        override protected Delegate RunDelegate => StartStory;

        private void StartStory(ulong userId, string storyId)
        {
            command?.RespondAsync("ALEX WHY IS IT BROKEN");
            // var controller = NarrativeController.Instance;
            // if (controller.StartNewNarrative(userId, storyId))
            // {
            //     var content = controller.ProgressStory(userId);
            //     command?.RespondAsync(content);
            // }
            // else
            // {
            //     command?.RespondAsync("oops");
            // }
        }
    }

    private class ChooseCommand : RunnableSlashCommand
    {
        override public string Name => "choose";
        override protected string Description => "Make a choice";
        override protected Delegate RunDelegate => ChooseOption;

        private void ChooseOption(ulong userId, int index)
        {
            var controller = NarrativeController.Instance;
            if (controller.MakeChoice(userId, index))
            {
                var content = controller.ProgressStory(userId);
                command?.RespondAsync(content);
            }
            else
            {
                command?.RespondAsync("oops");
            }
        }
    }
}