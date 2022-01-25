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

        try
        {
            await server.CreateApplicationCommandAsync(serverCommand.Build());
        }
        catch (HttpException httpException)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Critical, nameof(SlashCommandInstaller),
                "Failed to install slash commands", httpException));
        }

        client.SlashCommandExecuted += SlashCommandHandler;
    }

    private static async Task SlashCommandHandler(SocketSlashCommand slashCommand)
    {
        await slashCommand.RespondAsync($"You executed {slashCommand.CommandName}", ephemeral: true);
    }
}
