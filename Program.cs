// See https://aka.ms/new-console-template for more information

using DiscordTest;
using System;

DiscordBot bot = new DiscordBot();
await bot.Init();

Console.WriteLine("Closing bot!");
