using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
    [Capi.Cmd("%gel", 1, 1)]
    class gel : NormalCommand
    {


        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder tagss = new StringBuilder(a.Message.Replace(Command, ""));
            if (tagss.Length < 1)
            {
                await a.Send("You gotta tell me some tags too silly!!!");
                return;
            }
            while (tagss[0] == ' ')
                tagss.Remove(0, 1);
            while (tagss[^1] == ' ')
                tagss.Remove(tagss.Length - 1, 1);

            if (a.abbybotUser.userFavoriteCharacter != null) {
            var fc = a.abbybotUser.userFavoriteCharacter.FavoriteCharacter;
            tagss.Replace("&fc", $"{fc}*");
            }


            var tags = tagss.ToString().Split(' ').ToList();

            var sgc = (ITextChannel)a.channel;
            if (sgc == null) return;

            var blacklisttags = await UserBlacklistSql.GetBlackListTags(a.abbybotUser.Id);

            foreach (var item in blacklisttags)
            {
                tags.Add($"-{item}");
            }

            if (a.abbybotGuild != null) { 
                var ratings = a.abbybotUser.userPerms.Ratings;
                if (!sgc.IsNsfw || !a.abbybotUser.userFavoriteCharacter.IsLewd || !ratings.Contains(CommandRatings.hot))
                {
                    tags.Add("rating:safe");
                }
            }
            EmbedBuilder eb = null;
            try
            {
                BooruSharp.Search.Post.SearchResult imgdata = await service(tags);
                ImgData im = (new ImgData { });

                if (imgdata.FileUrl != null)
                {
                    im.Imageurl = imgdata.FileUrl.ToString();
                }
                if (imgdata.Source != null)
                {
                    im.source = imgdata.Source;
                }


                try
                {
                    eb = Contains.Gelbooru.embed.GelEmbed.Build(im, new StringBuilder("abbybot"));
                }
                catch
                {
                    eb = new EmbedBuilder { Description = "It didn't work... :(" };
                }
            }
            catch { }

            await a.Send(eb);
        }

        public virtual async Task<BooruSharp.Search.Post.SearchResult> service(List<string> tags)
        {
            return await Apis.Booru.AbbyBooru.Execute(tags.ToArray());
        }
    }
}
