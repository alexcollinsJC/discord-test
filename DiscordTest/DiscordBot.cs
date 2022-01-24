using Discord;
using Discord.WebSocket;
using System;
using Discord.Commands;

namespace DiscordTest
{
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
            await LoadCommands();
            Test();
        }

        private async Task LoadCommands()
        {
            CommandService commandService = new(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            CommandHandler commandHandler = new(client, commandService);
            await commandHandler.InstallCommandAsync();
        }

        private async void Test()
        {
            await Task.Delay(1000);
            await SendMessage();
        }

        public async Task SendMessage()
        {
            SocketChannel channel = client.GetChannel(934211004487852042);

            if (channel is IMessageChannel messageChannel)
            {
                Console.WriteLine("Sending message...");
                IUserMessage sentMessage = await messageChannel.SendMessageAsync("Hello World!");

                Console.WriteLine($"Message sent: {sentMessage}");
            }
        }
    }
}
