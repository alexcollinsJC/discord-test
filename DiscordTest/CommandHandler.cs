using Discord.Commands;
using System.Reflection;
using System.Text;
using Discord;
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
            commandService.CommandExecuted += OnCommandExecuted;

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            StringBuilder sb = new();
            sb.AppendLine("Installed Commands:");
            foreach (ModuleInfo moduleInfo in commandService.Modules)
            {
                // [module1]
                //   command1(param1, param2)
                //   command2(param3, param4)
                sb.AppendLine($"[{moduleInfo.Name}]");
                foreach (CommandInfo ci in moduleInfo.Commands)
                {
                    sb.AppendLine($"  {ci.Name}({string.Join(", ", ci.Parameters.Select(pi => $"{pi.Type.Name} {pi.Name}"))})");
                }
            }

            Console.Write(sb.ToString());
        }

        private static Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            string commandName = command.IsSpecified ? command.Value.Name : "A command";
            Console.WriteLine(new LogMessage(LogSeverity.Info,
                "CommandExecution",
                $"{commandName} was executed at {DateTime.UtcNow}."));

            return Task.CompletedTask;
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
