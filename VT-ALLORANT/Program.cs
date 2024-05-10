using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using VT_ALLORANT.Model;

namespace VT_ALLORANT;

class Program
{
    private BotBase botBase = new BotBase();

    public static void Main(string[] args)
    {
        new Program().MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {
        await botBase.Start();
        await Task.Delay(-1);
    }
}