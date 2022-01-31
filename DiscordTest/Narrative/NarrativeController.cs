namespace DiscordTest.Narrative;

sealed public class NarrativeController
{
    private static NarrativeController? instance;

    public static NarrativeController Instance
    {
        get { return instance ??= new NarrativeController(); }
    }

    private readonly Dictionary<ulong, NarrativeRunner> runnerMap = new Dictionary<ulong, NarrativeRunner>();

    public bool StartNewNarrative(ulong userId, string storyId)
    {
        if (runnerMap.ContainsKey(userId))
        {
            //TODO: better messaging
            return false;
        }
        else
        {
            //TODO: check story id 
            System.Console.WriteLine($"Starting new narrative!");
            var runner = new NarrativeRunner();
            runnerMap.Add(userId, runner);
            return true;
        }
    }

    public bool MakeChoice(ulong userId, int choice)
    {
        if (runnerMap.TryGetValue(userId, out var runner))
        {
            return runner.MakeChoice(choice);
        }
        else
        {
            return false;
        }
    }

    public (string, string[]) ProgressStory(ulong userId)
    {
        if(runnerMap.TryGetValue(userId, out var runner))
        {
            Console.WriteLine($"Progressing narrative for user {userId}");
            (string content, string[] choices) = runner.Run();
            if (runner.Done)
            {
                runnerMap.Remove(userId);
            }
            return (content, choices);
        }
        else
        {
            return ("No active narrative for user.", Array.Empty<string>());
        }
    }
}
