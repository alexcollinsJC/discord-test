// See https://aka.ms/new-console-template for more information

using DiscordTest;

DiscordBot bot = new();
await bot.Connect();

Console.WriteLine("Closing bot!");
