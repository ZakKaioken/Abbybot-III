using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Apis.Events
{
    public class Channel
    {
        public static async Task Created(SocketChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public static async Task Destroyed(SocketChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public static async Task Updated(SocketChannel oldchannel, SocketChannel newchannel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public static void Init(DiscordSocketClient _client)
        {
            _client.ChannelCreated += async (channel) => await Created(channel);
            _client.ChannelDestroyed += async (channel) => await Destroyed(channel);
            _client.ChannelUpdated += async (oldchannel, newchannel) => await Updated(oldchannel, newchannel);
        }
    }
}