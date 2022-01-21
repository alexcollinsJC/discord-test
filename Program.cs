// See https://aka.ms/new-console-template for more information

using DiscordTest;
using System;

DiscordBot bot = new DiscordBot();
await bot.Connect();

Console.WriteLine("Closing bot!");
