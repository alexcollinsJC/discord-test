using Discord;
using Discord.WebSocket;
using System;

namespace DiscordTest
{
    public class DiscordBot
    {
        private DiscordSocketClient _client;

        public DiscordBot()
        {
            _client = new DiscordSocketClient();
            
            _client.Log += Log;
            _client.Ready += OnReady;
        }

        public async Task Connect()
        {
            var token = System.Environment.GetEnvironmentVariable("DiscordBotToken");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
	        Console.WriteLine(msg.ToString());
	        return Task.CompletedTask;
        }

        // Put test code here
        private Task OnReady()
        {
            Test();
            return Task.CompletedTask;
        }

        private async void Test()
        {
            await Task.Delay(1000);
            await SendMessage();
        }

        public async Task SendMessage()
        {
            SocketChannel channel = _client.GetChannel(934211004487852042);

            if (channel is IMessageChannel messageChannel)
            {
                Console.WriteLine("Sending message...");
                IUserMessage sentMessage = await messageChannel.SendMessageAsync("Hello World!");

                Console.WriteLine($"Message sent: {sentMessage}");
            }
        }
    }
}