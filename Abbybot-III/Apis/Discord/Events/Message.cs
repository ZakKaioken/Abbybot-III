using Abbybot_III.Core;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Discord.Events
{
    internal class Message
    {
        internal static void Init(DiscordSocketClient _client)
        {
            _client.MessageReceived += async (message) => await Recieved(message);
            _client.MessageDeleted += async (message, channel) => await Deleted(message, channel);
            _client.MessageUpdated += async (oldmessage, newmessage, channel) => await Updated(oldmessage, newmessage, channel);
            _client.MessagesBulkDeleted += async (messages, channel) => await BulkDeleted(messages, channel);
        }

        private static async Task Recieved(SocketMessage message)
        {
            
            var guild = (message.Author is SocketGuildUser gu) ? gu.Guild.Name : "dms";
          
            var username = message.Author.Username;
      
            Console.WriteLine($"{guild}-{username}: {message.Content}");

            await CommandHandler.Handle(message);
        }

        private static async Task Deleted(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task Updated(Cacheable<IMessage, ulong> oldmessage, SocketMessage newmessage, ISocketMessageChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private static async Task BulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, ISocketMessageChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}