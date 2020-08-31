using Abbybot_III.Apis.Discord.Events;
using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.RequestSystem;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
    class GelbooruCommand : ContainCommand
    {

        public List<string> tags = new List<string> { "" };

        public GelbooruCommand(string Command, string[] tags, Capi.Interfaces.CommandRatings Rating)
        {
            this.Rating = Rating;
            this.Command = Command;
            List<string> s = new List<string>();
            this.tags = tags.ToList();
        }


        public override async Task DoWork(AbbybotCommandArgs msg)
        {
            //await msg.Send($"I heard you type the {Command} command...");
            List<AbbybotUser> mentionedUsers = new List<AbbybotUser>();
            foreach (var u in msg.mentionedUserIds) {
                AbbybotUser au = null;
                if (u is SocketGuildUser sgu)
                {
                    au = await AbbybotUser.GetUserFromSocketGuildUser(sgu);
                } else
                {
                    au = await AbbybotUser.GetUserFromSocketUser(u);
                }
                mentionedUsers.Add(au);
            }
            bool isnsf = true;
            if (msg.channel is ITextChannel itc)
                isnsf = itc.IsNsfw;


            ImgData im = (new ImgData { safe = isnsf || (Rating != ((CommandRatings)2)), command = Command, mentions = mentionedUsers, user = msg.abbybotUser });
            string url = "";

            im.sudouser = msg.abbybotSudoUser;
            List<object> os = new List<object>();
            int count = 1;
            var s = msg.Message.Split(" ");
            foreach (var e in s)
            {
                if (e.Length > 0)
                    if (e[0] == '~')
                    {
                        string f = e.Replace("~", "");
                        try
                        {
                            count = int.Parse(f);
                        }
                        catch (Exception et)
                        {
                            await msg.Send(et.ToString());
                        }

                    }
            }
            if (count < 0)
                count = 0;
            if (count > 4)
                count = 4;
            for (int t = 0; t < count; t++)
            {
                await PostImages(msg, im);

            }

            await Task.CompletedTask;

        }

        private async Task PostImages(AbbybotCommandArgs msg, ImgData im)
        {
            await GetImage(im);
            if (im.source == null)
                im.source = "no source";
            EmbedBuilder data = null;
            if (!im.source.Contains("error"))
                data = embed.GelEmbed.Build(im, sb);
            else if (im.source.Contains("errornsfw"))
            {
                await msg.Send("master i can't search for nsfw pictures in a safe channel. there may be children.");
                return;
            }
            else
            {
                var fc = im.user.userFavoriteCharacter.FavoriteCharacter;
                await msg.Send($"sorry master... I could not find the an {im.command.Replace("%", "")}ing picture for {fc}.. im sory");

                return;
            }
            //object returnobj = null;
            if (msg.abbybotGuild != null)
            {

                bool loli = (msg.abbybotGuild.NoLoli && im.loli);
                bool shot = (msg.abbybotGuild.NoLoli && im.shot);
                bool nsfw = (msg.abbybotGuild.NoNSFW && im.nsfw);
                if (loli || shot || nsfw)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Autodestructing in 2 minutes due to this server not allowing ");

                    if (loli)
                        sb.Append("lolis");
                    if (loli && shot)
                        sb.Append(", ");
                    if (shot)
                        sb.Append("shotas");
                    if ((loli || shot) && nsfw)
                        sb.Append(" and ");
                    if (nsfw)
                        sb.Append("nsfw");

                    (data).Footer = new EmbedFooterBuilder() { Text = sb.ToString() };

                    var reqj = new RequestObject();
                    reqj.itc = msg.channel;

                    reqj.o = data;
                    reqj.UsersMessage = msg.originalMessage;
                    reqj.requestType = RequestType.Delete;
                    //await msg.Send($"I sending command as a request...");
                    await msg.Send(reqj);
                }
                else
                {
                    //await msg.Send($"sending immediately");
                    await msg.Send(data);
                }
            }
        }

        private async Task GetImage(ImgData im)
        {
            try
            {
                var blacklisttags = await UserBlacklistSql.GetBlackListTags(im.user.Id);
                var tagz = tags.ToList();
                foreach (var item in blacklisttags)
                {
                    tagz.Add($"-{item}");
                }
                BooruSharp.Search.Post.SearchResult imgdata;

                try
                {
                    imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());
                }
                catch
                {
                    im.source = "error";
                    return;
                }


                im.loli = imgdata.tags.Contains("loli");
                im.shot = imgdata.tags.Contains("shot");
                im.nsfw = (imgdata.rating == BooruSharp.Search.Post.Rating.Explicit || imgdata.rating == BooruSharp.Search.Post.Rating.Explicit);
                //if (!tags.Contains("ratings:safe")) im.nsfw = true; 
                //Console.WriteLine(imgdata.rating);


                if (imgdata.fileUrl != null)
                {
                    im.Imageurl = imgdata.fileUrl.ToString();
                }
                if (imgdata.source != null)
                {
                    im.source = imgdata.source;
                }
            }
            catch (BooruSharp.Search.InvalidTags)
            {
                if (im.safe)
                    im.source = "error";
                else
                    im.source = "errornsfw";
            }
        }


        public GelbooruCommand()
        {

        }

        StringBuilder sb = new StringBuilder();
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            bool canrun = await CanRun(aca);
            //Console.WriteLine($"evaluating {Command}");
            if (aca.abbybotUser.userTrust.inTimeOut && canrun)
            {
                await aca.Send($"You're in timeout for a little while. You did a mean thing and I can't stand for that. Check your time and details with %timeout. Sorry.");
                canrun = false;
            }
            return canrun;
        }

        private async Task<bool> CanRun(AbbybotCommandArgs md)
        {
            bool canrun = false;

            if (!md.Message.Contains(Command))
                return false;

            bool isnsfwchannel = await md.IsNSFW();

            if (tags.Count <= 0)
                return false;

            List<string> taggs = new List<string>(tags);
            bool safe = false;

            var e = md.author;
            var d = md.channel;

            if (e is SocketGuildUser r && d is ITextChannel f)
            {
                if (!md.abbybotUser.userPerms.Ratings.Contains((CommandRatings)2))
                {
                    safe = true;
                }
                if (!md.abbybotUser.userPerms.Ratings.Contains((CommandRatings)3))
                {

                    taggs.Add("-loli");
                }
            }
            if (!isnsfwchannel || !md.abbybotUser.userFavoriteCharacter .IsLewd || safe)
            {
                taggs.Add("rating:safe");
            }


            if (md.abbybotSudoUser == null)
            {

                Random abbyrng = new Random();
                sb.Clear();
                if (md.mentionedUserIds.Count > 0)
                {
                    var rng = abbyrng.Next(0, md.mentionedUserIds.Count);
                    var userx = await AbbybotUser.GetUserFromSocketUser(md.mentionedUserIds[rng]);
                    sb.Append(userx.userFavoriteCharacter.FavoriteCharacter);
                }
                else
                {
                    sb.Append((md.abbybotUser.userFavoriteCharacter. FavoriteCharacter.Length > 1) ? md.abbybotUser.userFavoriteCharacter.FavoriteCharacter : "abigail_williams");
                }

                sb.Replace("abbybot", "abigail_williams");

                taggs.Add($"{sb}*");
            }
            else
            {
                taggs.Add("abigail_williams*");
            }
            tags = taggs.ToList();

            if (md.channel is ITextChannel sgc)
            {
                if (md.author is SocketGuildUser sgu)
                {
                    //var ratings = await RoleManager.GetRatings(sgu, sgc);

                    if (md.abbybotUser.userPerms.Ratings.Contains(Rating))
                    {
                        canrun = true;
                    }
                }
            }
            else if (md.channel is SocketDMChannel)
            {
                canrun = true;
            }

            await Task.CompletedTask;
            return canrun;
        }


    }
}
