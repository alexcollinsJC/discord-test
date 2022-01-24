using Discord.Net;
using Discord.Commands;
using System;

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

            await commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }
    }
}