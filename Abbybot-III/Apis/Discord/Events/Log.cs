using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
     internal class Log
    {
         internal static void Init(DiscordSocketClient _client)
        {
            _client.Log += async (log) => await Log.Recieved(log);
        }

         static async Task Recieved(LogMessage log)
        {
            await Task.CompletedTask;
            Console.WriteLine(log.ToString());
            //throw new NotImplementedException();
        }
    }
}