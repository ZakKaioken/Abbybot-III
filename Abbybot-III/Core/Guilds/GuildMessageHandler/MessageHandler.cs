using Abbybot_III.Core.Guilds.DataType;

using Discord;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler
{
    class MessageHandler
    {
        public static async Task DoGuildMessage(GuildMessage gm)
        {
            if (gm == null)
                return;
            EmbedBuilder embedBuilder = new EmbedBuilder()
            {
                Color = gm.color,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "abbybot"
                }
            };
            if (gm.imgurl != "")
                embedBuilder.ImageUrl = gm.imgurl;

            embedBuilder.Title = gm.message;
            var channel = gm.guild.GetTextChannel(gm.channelId);
            Embed emb = embedBuilder.Build();
            await channel.SendMessageAsync(null, false, emb);
        }
    }
}