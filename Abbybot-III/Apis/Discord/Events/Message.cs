using Abbybot_III.Core;
using Abbybot_III.Core.CommandHandler;
using Abbybot_III.Sql.Abbybot.Abbybot;
using Abbybot_III.Sql.Abbybot.User;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            bool nowrite = false;
            var cs = await AbbybotSql.GetAbbybotChannelIdAsync();
            foreach (var (guildId, channelId) in cs) {
            if (message.Channel.Id != channelId)
                nowrite = true;
            }
            if (nowrite)
                WriteMessage(message);


            await PassiveUserSql.IncStat(message.Author.Id, "MessagesSent");
            await CommandHandler.Handle(message);
        }

        private static void WriteMessage(SocketMessage message)
        {
            var guild = (message.Author is SocketGuildUser gu) ? gu.Guild.Name : "dms";

            var username = message.Author.Username;
            StringBuilder sb = new StringBuilder();
            sb.Append(guild).Append("-").Append(username).Append(": ");

            sb.AppendLine(message.Content);
            
            foreach (var embed in message.Embeds.ToList())
                {
                    if (embed.Title is string)
                    sb.AppendLine($"[{embed.Title}]");
                    
                    if (embed.Description is string)
                        sb.AppendLine($"[{embed.Description}]");
                    if (embed.Image.HasValue)
                        sb.AppendLine($"[{embed.Image.Value.Url}]");
                    if (embed.Video.HasValue)
                        sb.AppendLine($"[{embed.Video.Value}]");
                    foreach (var field in embed.Fields)
                    {
                        sb.AppendLine($"-[{field.Name}]");
                        sb.AppendLine($"-[{field.Value}]");
                    }
                    if (embed.Footer.HasValue) {
                        var foot = embed.Footer.Value;
                        sb.AppendLine($"[{foot.Text}]");
                    }
                }
            
            Abbybot.print(sb.ToString());
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