using System.Text;
using Ink.Runtime;

namespace DiscordTest.Narrative;

public class NarrativeRunner
{
    private const string DefaultStory = "{\"inkVersion\":20,\"root\":[[[\"^I looked at Monsieur Fogg\",\"\n\",[\"ev\",{\"^->\":\"0.g-0.2.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-0\",\"flg\":18},{\"s\":[\"^... and I could contain myself no longer.\",{\"->\":\"$r\",\"var\":true},null]}],[\"ev\",{\"^->\":\"0.g-0.3.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-1\",\"flg\":18},{\"s\":[\"^... but I said nothing\",{\"->\":\"$r\",\"var\":true},null]}],{\"c-0\":[\"ev\",{\"^->\":\"0.g-0.c-0.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.2.s\"},[{\"#n\":\"$r2\"}],\"\n\",\"^'What is the purpose of our journey, Monsieur?'\",\"\n\",\"^'A wager,' he replied.\",\"\n\",[[\"ev\",{\"^->\":\"0.g-0.c-0.11.0.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-0\",\"flg\":18},{\"s\":[\"^'A wager!'\",{\"->\":\"$r\",\"var\":true},null]}],[\"ev\",{\"^->\":\"0.g-0.c-0.11.1.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"str\",\"^.'\",\"/str\",\"/ev\",{\"*\":\".^.^.c-1\",\"flg\":22},{\"s\":[\"^'Ah\",{\"->\":\"$r\",\"var\":true},null]}],{\"c-0\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.0.s\"},[{\"#n\":\"$r2\"}],\"^ I returned.\",\"\n\",\"^He nodded.\",\"\n\",[[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.0.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-0\",\"flg\":18},{\"s\":[\"^'But surely that is foolishness!'\",{\"->\":\"$r\",\"var\":true},null]}],[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.1.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-1\",\"flg\":18},{\"s\":[\"^'A most serious matter then!'\",{\"->\":\"$r\",\"var\":true},null]}],{\"c-0\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.c-0.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.0.s\"},[{\"#n\":\"$r2\"}],\"\n\",{\"->\":\".^.^.g-0\"},{\"#f\":5}],\"c-1\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.c-1.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.1.s\"},[{\"#n\":\"$r2\"}],\"\n\",{\"->\":\".^.^.g-0\"},{\"#f\":5}],\"g-0\":[\"^He nodded again.\",\"\n\",[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.2.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-2\",\"flg\":18},{\"s\":[\"^'But can we win?'\",{\"->\":\"$r\",\"var\":true},null]}],[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.3.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"/ev\",{\"*\":\".^.^.c-3\",\"flg\":18},{\"s\":[\"^'A modest wager, I trust?'\",{\"->\":\"$r\",\"var\":true},null]}],[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.4.$r1\"},{\"temp=\":\"$r\"},\"str\",{\"->\":\".^.s\"},[{\"#n\":\"$r1\"}],\"/str\",\"str\",\"^.\",\"/str\",\"/ev\",{\"*\":\".^.^.c-4\",\"flg\":22},{\"s\":[\"^I asked nothing further of him then\",{\"->\":\"$r\",\"var\":true},null]}],{\"c-2\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.c-2.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.2.s\"},[{\"#n\":\"$r2\"}],\"\n\",\"^'That is what we will endeavour to find out,' he answered.\",\"\n\",{\"->\":\".^.^.^.^.^.g-0\"},{\"#f\":5}],\"c-3\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.c-3.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.3.s\"},[{\"#n\":\"$r2\"}],\"\n\",\"^'Twenty thousand pounds,' he replied, quite flatly.\",\"\n\",{\"->\":\".^.^.^.^.^.g-0\"},{\"#f\":5}],\"c-4\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-0.10.g-0.c-4.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.4.s\"},[{\"#n\":\"$r2\"}],\"^, and after a final, polite cough, he offered nothing more to me. \",\"<>\",\"\n\",{\"->\":\".^.^.^.^.^.g-0\"},{\"#f\":5}],\"#f\":5}]}],{\"#f\":5}],\"c-1\":[\"ev\",{\"^->\":\"0.g-0.c-0.11.c-1.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.1.s\"},[{\"#n\":\"$r2\"}],\"^,' I replied, uncertain what I thought.\",\"\n\",{\"->\":\".^.^.g-0\"},{\"#f\":5}],\"g-0\":[\"^After that, \",\"<>\",\"\n\",{\"->\":\"0.g-1\"},{\"#f\":5}]}],{\"#f\":5}],\"c-1\":[\"ev\",{\"^->\":\"0.g-0.c-1.$r2\"},\"/ev\",{\"temp=\":\"$r\"},{\"->\":\".^.^.3.s\"},[{\"#n\":\"$r2\"}],\"^ and \",\"<>\",\"\n\",{\"->\":\"0.g-1\"},{\"#f\":5}],\"#f\":5,\"#n\":\"g-0\"}],{\"g-1\":[\"^we passed the day in silence.\",\"\n\",[\"end\",[\"done\",{\"#f\":5,\"#n\":\"g-3\"}],{\"#f\":5,\"#n\":\"g-2\"}],{\"#f\":5}]}],\"done\",{\"#f\":1}],\"listDefs\":{}}";
    private readonly Story inkStory;

    public bool Done => inkStory.currentChoices.Count == 0;
    
    public NarrativeRunner()
    {
        inkStory = new Story(DefaultStory);
    }

    public (string, string[]) Run()
    {
        string text = inkStory.ContinueMaximally();
        List<string> choices = new();
        foreach (Choice choice in inkStory.currentChoices)
        {
            choices.Add(choice.text);
        }

        return (text, choices.ToArray());
    }

    public bool MakeChoice(int choice)
    {
        try
        {
            inkStory.ChooseChoiceIndex(choice);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
