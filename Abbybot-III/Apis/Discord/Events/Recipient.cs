using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    public class Recipient
    {
        public static void Init(DiscordSocketClient _client)
        {
            _client.RecipientAdded += async (recipient) => await Recipient.Added(recipient);
            _client.RecipientRemoved += async (recipient) => await Recipient.Removed(recipient);
        }

        static async Task Added(SocketGroupUser recipient)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Removed(SocketGroupUser recipient)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}