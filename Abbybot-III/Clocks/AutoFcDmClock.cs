using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    class AutoFcDmClock : RepeatingClock
    {
        public override async Task OnInit(DateTime time)
        {
            name = "autofcdm";
            delay = TimeSpan.FromMinutes(15);
            await base.OnInit(time);
        }
         
        public override async Task OnWork(DateTime time)
        {
            foreach (var a in await AutoFcDmSqls.GetListAutoFcDmsAsync())
            {
                var u = Apis.Discord.Discord._client.GetUser(a);
                var b = await u.GetOrCreateDMChannelAsync();
                var au = await AbbybotUser.GetUserFromSocketUser(u);

                var fc = au.userFavoriteCharacter.FavoriteCharacter;
                List<string> tagz = new List<string>();
                tagz.Add(fc);
                var blacklisttags = await UserBlacklistSql.GetBlackListTags(au.Id);
                foreach (var item in blacklisttags)
                {
                    tagz.Add($"-{item}");
                }
                var imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());
                string fileurl = imgdata.FileUrl.ToString();
                string source = imgdata.Source;

                var e = GelEmbed.Build(fileurl, source, fc).Build();
                await b.SendMessageAsync(null, false, e);
            }
        }
    }
}
