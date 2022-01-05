using Abbybot_III.Commands.Contains.GelbooruV4;
using Abbybot_III.Commands.Contains.GelbooruV4.embed;
using Abbybot_III.Sql.Abbybot.AbbyBooru;

using Discord;
using Discord.WebSocket;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Events
{
    public class Reaction
    {
        public static void Init(DiscordSocketClient _client)
        {
            _client.ReactionAdded += async (message, channel, reaction) => await Reaction.Added(message, channel, reaction);
            _client.ReactionRemoved += async (message, channel, reaction) => await Reaction.Removed(message, channel, reaction);
            _client.ReactionsCleared += async (message, channel) => await Reaction.Cleared(message, channel);
        }

        static async Task Added(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {

            var msg = await message.GetOrDownloadAsync();
            var cha = await channel.GetOrDownloadAsync();
            await GelbooruEmojiEngine.DoEmojiReaction(msg,cha, reaction);

        }

        static async Task Removed(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Cleared(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}