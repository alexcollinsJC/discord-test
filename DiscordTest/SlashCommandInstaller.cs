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

    public async Task InstallSlashCommands(DiscordSocketClient client)
    {
        SocketGuild? server = client.GetGuild(TestServerGuildId);
        if (server == null) return;

        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(BaseSlashCommand)) && !type.IsAbstract)
            {
                if (Activator.CreateInstance(type) is BaseSlashCommand instance)
                {
                    slashCommands.Add(instance);
                }
            }
        }

        try
        {
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
                string required = option.IsRequired.HasValue && option.IsRequired.Value ? " (required)" : string.Empty;
                sb.AppendLine($"  ({option.Type}) {option.Name} -- {option.Description}{required}");
            }
        }

        Console.Write(sb.ToString());

        client.SlashCommandExecuted += SlashCommandHandler;
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        Console.WriteLine(new LogMessage(LogSeverity.Info,
            nameof(SlashCommandInstaller),
            $"Handling command '{GetCommandString(command.Data)}'"));

        foreach (BaseSlashCommand slashCommand in slashCommands)
        {
            if (command.CommandName == slashCommand.Name)
            {
                await slashCommand.Run(command);
            }
        }
    }

    private static string GetCommandString(SocketSlashCommandData data)
    {
        List<string> commandParts = new()
        {
            $"/{data.Name}"
        };

        GetCommandString(commandParts, data.Options);

        return string.Join(" ", commandParts);
    }

    private static void GetCommandString(List<string> parts, IEnumerable<SocketSlashCommandDataOption> dataOption)
    {
        foreach (SocketSlashCommandDataOption option in dataOption)
        {
            parts.Add(option.Name);
            if (option.Type is ApplicationCommandOptionType.SubCommand or ApplicationCommandOptionType.SubCommandGroup)
            {
                GetCommandString(parts, option.Options);
            }
            else
            {
                parts.Add($": {option.Value}");
            }
        }
    }
}
