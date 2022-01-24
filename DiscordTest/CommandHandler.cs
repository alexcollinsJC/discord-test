using Discord.Commands;
using System.Reflection;
using Discord.WebSocket;

namespace DiscordTest
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;

        public CommandHandler(DiscordSocketClient discordClient, CommandService commands)
        {
            client = discordClient;
            commandService = commands;
        }

        public async Task InstallCommandAsync()
        {
            client.MessageReceived += HandleCommandAsync;

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (message is not SocketUserMessage userMessage) return;

            int argPos = 0;
            if (userMessage.Author.IsBot || !(userMessage.HasCharPrefix('!', ref argPos) ||
                                              userMessage.HasMentionPrefix(client.CurrentUser, ref argPos)))
            {
                return;
            }

            SocketCommandContext context = new(client, userMessage);
            await commandService.ExecuteAsync(context, argPos, null);
        }
    }
}
