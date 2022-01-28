using System.Reflection;
using System.Text;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordTest.Slash;

namespace DiscordTest;

public class SlashCommandInstaller
{
    private const ulong TestServerGuildId = 934204465484025907;

    private List<BaseSlashCommand> slashCommands = new();

    public async Task InstallSlashCommands(DiscordSocketClient client, bool purgeCommands)
    {
        SocketGuild? server = client.GetGuild(TestServerGuildId);
        if (server == null) return;

        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(BaseSlashCommand)) && !type.IsAbstract && !type.IsNested)
            {
                if (Activator.CreateInstance(type) is BaseSlashCommand instance)
                {
                    slashCommands.Add(instance);
                }
            }
        }

        try
        {
            if (purgeCommands)
            {
                await server.DeleteApplicationCommandsAsync();
            }

            foreach (BaseSlashCommand slashCommand in slashCommands)
            {
                await server.CreateApplicationCommandAsync(slashCommand.Build());
            }
        }
        catch (HttpException httpException)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Critical, nameof(SlashCommandInstaller),
                "Failed to install slash commands", httpException));
        }

        StringBuilder sb = new();
        sb.AppendLine("Installed Slash Commands:");
        foreach (SocketApplicationCommand? slashCommand in await server.GetApplicationCommandsAsync())
        {
            // [command -- description]
            //   (type) option1 -- description (required)
            //   (type) option2 -- description
            sb.AppendLine($"[{slashCommand.Name} -- {slashCommand.Description}]");
            foreach (SocketApplicationCommandOption? option in slashCommand.Options)
            {
                if (option is null) continue;
                GetCommandString(option, sb, 1);
            }
        }

        Console.Write(sb.ToString());

        client.SlashCommandExecuted += SlashCommandHandler;
    }

    private static void GetCommandString(SocketApplicationCommandOption option, StringBuilder sb, int depth)
    {
        string indent = string.Concat(Enumerable.Repeat(" ", depth));
        if (option.Type is ApplicationCommandOptionType.SubCommandGroup or ApplicationCommandOptionType.SubCommand)
        {
            sb.AppendLine($"{indent}<{option.Name} -- {option.Description}>");
            foreach (SocketApplicationCommandOption commandOption in option.Options)
            {
                GetCommandString(commandOption, sb, depth + 1);
            }
        }
        else
        {
            string required = option.IsRequired.HasValue && option.IsRequired.Value ? " (required)" : string.Empty;
            sb.AppendLine($"{indent}({option.Type}) {option.Name} -- {option.Description}{required}");
        }
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        Console.WriteLine(new LogMessage(LogSeverity.Info,
            nameof(SlashCommandInstaller),
            $"Handling command '{GetCommandDataString(command.Data)}'"));

        try
        {
            BaseSlashCommand? slashCommand = slashCommands.Find(c => c.Name == command.CommandName);
            if (slashCommand is not null)
            {
                await slashCommand.Run(command);
            }
            else
            {
                throw new ArgumentException($"Unknown command {GetCommandDataString(command.Data)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static string GetCommandDataString(SocketSlashCommandData data)
    {
        List<string> commandParts = new()
        {
            $"/{data.Name}"
        };

        GetCommandDataString(commandParts, data.Options);

        return string.Join(" ", commandParts);
    }

    private static void GetCommandDataString(List<string> parts, IEnumerable<SocketSlashCommandDataOption> dataOption)
    {
        foreach (SocketSlashCommandDataOption option in dataOption)
        {
            parts.Add(option.Name);
            if (option.Type is ApplicationCommandOptionType.SubCommand or ApplicationCommandOptionType.SubCommandGroup)
            {
                GetCommandDataString(parts, option.Options);
            }
            else
            {
                parts.Add($": {option.Value}");
            }
        }
    }
}
