using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTest;

public class DiscordBot
{
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
        await Task.Delay(-1);
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
            AddSingleton<CommandHandler>().BuildServiceProvider();
        CommandHandler? handler = provider.GetService<CommandHandler>();
        if (handler != null)
        {
            await handler.InitializeAsync();
        }

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
