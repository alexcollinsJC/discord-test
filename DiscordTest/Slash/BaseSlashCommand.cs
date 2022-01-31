using System.Reflection;
using Discord;
using Discord.WebSocket;
using JetBrains.Annotations;

namespace DiscordTest.Slash;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class BaseSlashCommand
{
    public abstract string Name { get; }
    protected abstract string Description { get; }
    private SocketSlashCommand? command = null;

    protected SocketSlashCommand Command
    {
        get
        {
            if (command != null) return command;

            throw new NullReferenceException();
        }
    }

    private BaseSlashCommand[]? subCommands = null;

    private IEnumerable<BaseSlashCommand> SubCommands => subCommands ??= GetSubCommands().ToArray();

    protected virtual IEnumerable<BaseSlashCommand> GetSubCommands()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        foreach (Type nestedType in GetType().GetNestedTypes(flags))
        {
            if (nestedType.IsSubclassOf(typeof(BaseSlashCommand)) &&
                Activator.CreateInstance(nestedType) is BaseSlashCommand baseSlashCommand)
            {
                yield return baseSlashCommand;
            }
        }
    }

    public SlashCommandProperties Build() => GetBuilder().Build();

    protected virtual SlashCommandBuilder GetBuilder()
    {
        SlashCommandBuilder builder = new();
        builder.WithName(Name).WithDescription(Description);
        foreach (BaseSlashCommand subCommand in SubCommands)
        {
            SlashCommandBuilder subProps = subCommand.GetBuilder();

            SlashCommandOptionBuilder subOptionBuilder = new SlashCommandOptionBuilder().WithName(subProps.Name).
                WithDescription(subProps.Description).WithType(subCommand is RunnableSlashCommand ?
                    ApplicationCommandOptionType.SubCommand :
                    ApplicationCommandOptionType.SubCommandGroup);

            subProps.Options?.ForEach(option => subOptionBuilder.AddOption(option));
            builder.AddOption(subOptionBuilder);
        }

        return builder;
    }

    public Task Run(SocketSlashCommand slashCommand)
    {
        command = slashCommand;
        return RunCommand(slashCommand.Data.Options, slashCommand.CommandName);
    }

    protected virtual Task RunCommand(IEnumerable<SocketSlashCommandDataOption> options, string path)
    {
        // There will be only one option -- the subcommand to navigate to next
        foreach (SocketSlashCommandDataOption option in options)
        {
            foreach (BaseSlashCommand slashCommand in SubCommands)
            {
                if (slashCommand.Name == option.Name)
                {
                    slashCommand.command = command;
                    return slashCommand.RunCommand(option.Options, $"{path} {slashCommand.Name}");
                }
            }

            throw new ArgumentException($"Error -- unable to parse command at : {path} {option.Name}. Available commands:\n" +
                                        $"{string.Join("\n", SubCommands.Select(sc => $"{Name} -- {Description}"))}");
        }

        return Task.CompletedTask;
    }
}
