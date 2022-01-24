// See https://aka.ms/new-console-template for more information

using DiscordTest;

DiscordBot bot = new DiscordBot();
await bot.Connect();

Console.WriteLine("Closing bot!");
