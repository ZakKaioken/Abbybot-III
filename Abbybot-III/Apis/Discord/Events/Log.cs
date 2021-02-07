using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
     public class Log
    {
         public static void Init(DiscordSocketClient _client)
        {
            _client.Log += async (log) => await Log.Recieved(log);
        }

         static async Task Recieved(LogMessage log)
        {
            await Task.CompletedTask;
            Abbybot.print(log.ToString());
            //throw new NotImplementedException();
        }
    }
}