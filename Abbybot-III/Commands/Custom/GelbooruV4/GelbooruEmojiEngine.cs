using Abbybot_III.Commands.Contains.GelbooruV4.embed;
using Abbybot_III.Sql.Abbybot.AbbyBooru;

using AbbySql;

using Discord;
using Discord.WebSocket;

using Nano.XML;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.GelbooruV4
{
	internal class GelbooruEmojiEngine
	{
	public static async Task DoEmojiReaction(IUserMessage msg, IMessageChannel channel, SocketReaction reaction) {
            var olo = await GelEmojiSql.GetEmojiType(msg.Id, reaction.Emote.Name, reaction.UserId);

            if (olo == -1) return;

            if (olo == 0)
            {
                var o = await GelEmojiSql.GetEmojiCommand(msg.Id, reaction.Emote.Name, reaction.UserId);
                if (o != null)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<GelbooruCommand>(o);
                    var results = await obj.GenerateAsync();
                    var embed = GelEmbed.GlobalBuild(obj, results);
                    var newpicture = await channel.SendMessageAsync(embed: embed.Build());
                    await obj.AddReactionsAsync((Discord.Rest.RestUserMessage)newpicture, results);
                }
            }
            else if (olo == 1)
            {
                var o = await GelEmojiSql.GetEmojiResult(msg.Id, reaction.Emote.Name, reaction.UserId);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Post>(o);
                EmbedBuilder eb = new();
                eb.Title = ($"here are the tags ♥️");
                StringBuilder sb = new();
                foreach (var tag in obj.Tags)
                {
                    sb.Append($"[**{EscapeMD(tag)}**] ");
                }
                eb.AddField("♥️", sb.ToString());
                eb.Color = Color.LightOrange;
                var newpicture = await channel.SendMessageAsync(embed: eb.Build());
            }
            else if (olo == 3)
            {
                var o = await GelEmojiSql.GetEmojiResult(msg.Id, reaction.Emote.Name, reaction.UserId);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Post>(o);
                
                var newpicture = await channel.SendMessageAsync(obj.fileUrl);
            }
            else if (olo == 2)
            {
                await msg.DeleteAsync();
            }
        }

		private static string EscapeMD(string tag)
		{
            return tag.Replace("_", "\\_");
		}
	}
}
