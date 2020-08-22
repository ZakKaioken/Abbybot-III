using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Channel
    {
        internal static Task Created(SocketChannel channel)
        {
            throw new NotImplementedException();
        }

        internal static Task Destroyed(SocketChannel channel)
        {
            throw new NotImplementedException();
        }

        internal static Task Updated(SocketChannel oldchannel, SocketChannel newchannel)
        {
            throw new NotImplementedException();
        }

        internal static void Init(DiscordSocketClient client)
        {
            throw new NotImplementedException();
        }
    }
}