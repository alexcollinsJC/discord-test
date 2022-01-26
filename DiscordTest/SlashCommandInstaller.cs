using System.Text;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace DiscordTest;

public class SlashCommandInstaller
{
    private const ulong TestServerGuildId = 934204465484025907;
    public async Task InstallSlashCommands(DiscordSocketClient client)
    {
        SocketGuild? server = client.GetGuild(TestServerGuildId);
        if (server == null) return;

        SlashCommandBuilder serverCommand = new();
        serverCommand.WithName("test-command").WithDescription("Test command description.");

        SlashCommandBuilder squareCommand = new();
        squareCommand.WithName("square").WithDescription("Square a number.").AddOption("number",
            ApplicationCommandOptionType.Number, "The number to square.", true);

        try
        {
            await server.CreateApplicationCommandAsync(serverCommand.Build());
            await server.CreateApplicationCommandAsync(squareCommand.Build());
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

    private static async Task SlashCommandHandler(SocketSlashCommand slashCommand)
    {
        Console.WriteLine(new LogMessage(LogSeverity.Info,
            nameof(SlashCommandInstaller),
            $"Handling command '{GetCommandString(slashCommand.Data)}'"));

        await (slashCommand.Data.Name switch
        {
            "test-command" => slashCommand.RespondAsync($"You executed {slashCommand.CommandName}", ephemeral: true),
            "square" => HandleSquareCommand(slashCommand),
            _ => Task.CompletedTask
        });
    }

    private static Task HandleSquareCommand(SocketSlashCommand command)
    {
        double value = 0;
        foreach (SocketSlashCommandDataOption option in command.Data.Options)
        {
            if (option.Name == "number")
            {
                value = (double) option.Value;
                break;
            }
        }

        return command.RespondAsync($"The square of '{value}' is '{value * value}'", ephemeral: true);
    }

    private static string GetCommandString(SocketSlashCommandData data)
    {
        StringBuilder sb = new($"/{data.Name}");
        foreach (SocketSlashCommandDataOption? option in data.Options)
        {
            if (option == null) continue;
            sb.Append($" {option.Name}: {option.Value}");
        }

        return sb.ToString();
    }
}
