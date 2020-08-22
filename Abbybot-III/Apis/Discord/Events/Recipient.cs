using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Recipient
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.RecipientAdded += async (recipient) => await Recipient.Added(recipient);
            _client.RecipientRemoved += async (recipient) => await Recipient.Removed(recipient);
        }

        private static async Task Added(SocketGroupUser recipient)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Removed(SocketGroupUser recipient)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}