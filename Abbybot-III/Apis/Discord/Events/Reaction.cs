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

        static async Task Added(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var msg = await message.GetOrDownloadAsync();
            Console.WriteLine($"emoji {reaction.Emote.Name}");
            var olo = await GelEmojiSql.GetEmojiType(msg.Id, reaction.Emote.Name, reaction.UserId);
            if (olo == 0) {
                var o = await GelEmojiSql.GetEmojiCommand(msg.Id, reaction.Emote.Name, reaction.UserId);
                if (o != null)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<GelbooruCommand>(o);
                    var results = await obj.GenerateAsync();
                    var embed = GelEmbed.GlobalBuild(obj, results);
                    var newpicture = await channel.SendMessageAsync(embed: embed.Build());
                    await obj.AddReactionsAsync(newpicture, results);
                }
            } else if (olo == 1) {
                var o = await GelEmojiSql.GetEmojiResult(msg.Id, reaction.Emote.Name, reaction.UserId);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<GelbooruResult>(o);
                EmbedBuilder eb = new();
                eb.Title = ($"here are the tags ♥️");
                StringBuilder sb = new();
                foreach(var tag in obj.tags) {
                    sb.Append($"[{tag}] ");
				}
                eb.AddField("♥️", sb.ToString());
                eb.Color = Color.LightOrange;
                var newpicture = await channel.SendMessageAsync(embed: eb.Build());
            } else if (olo == 2) {
                await msg.DeleteAsync();
			}

        }

        static async Task Removed(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }

        static async Task Cleared(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel)
        {
            await Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}