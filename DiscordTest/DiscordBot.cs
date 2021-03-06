using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTest;

public class DiscordBot
{
    private const int MaxLifetimeMs = 1000 * 60 * 60; // one hour
    private DiscordSocketClient client;

    public DiscordBot()
    {
        client = new DiscordSocketClient();

        client.Log += Log;
        client.Ready += OnReady;
    }

    public async Task Connect()
    {
        string? token = Environment.GetEnvironmentVariable("DiscordBotToken");

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(MaxLifetimeMs);

        await client.StopAsync();
        await client.LogoutAsync();
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    // Put test code here
    private async Task OnReady()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        CommandService commandService = new(new CommandServiceConfig
        {
            LogLevel = LogSeverity.Verbose
        });

        IServiceProvider provider = new ServiceCollection().AddSingleton(client).AddSingleton(commandService).
            AddSingleton<CommandHandler>().AddSingleton<SlashCommandInstaller>().BuildServiceProvider();

        await (provider.GetService<CommandHandler>()?.InitializeAsync() ?? Task.CompletedTask);
        await (provider.GetService<SlashCommandInstaller>()?.InstallSlashCommands(client, false) ?? Task.CompletedTask);

        await SendReadyMessage();
    }

    private async Task SendReadyMessage()
    {
        if (client.GetChannel(934211004487852042) is IMessageChannel messageChannel)
        {
            await messageChannel.SendMessageAsync("Discord Test Bot online!");
        }
    }
}
