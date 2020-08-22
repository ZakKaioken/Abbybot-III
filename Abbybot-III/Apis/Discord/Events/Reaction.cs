using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Reaction
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.ReactionAdded += async (message, channel, reaction) => await Reaction.Added(message, channel, reaction);
            _client.ReactionRemoved += async (message, channel, reaction) => await Reaction.Removed(message, channel, reaction);
            _client.ReactionsCleared += async (message, channel) => await Reaction.Cleared(message, channel);
        }

        private static async Task Added(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Removed(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Cleared(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}