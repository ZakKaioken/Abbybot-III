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

        public GelbooruCommand(string Command, string[] tags, CommandRatings Rating)
        {
            this.Rating = Rating;
            this.Command = Command;
            List<string> s = new List<string>();
            this.tags = tags.ToList();
        }


        public override async Task DoWork(AbbybotCommandArgs msg)
        {
            List<AbbybotUser> mentionedUsers = await GetMentionedUsers(msg);

            bool isnsf = IsChannelNsfw(msg);

            ImgData im = new ImgData
            {
                safe = isnsf || (Rating != ((CommandRatings)2)),
                command = Command,
                mentions = mentionedUsers,
                user = msg.abbybotUser
            };
            im.sudouser = msg.abbybotSudoUser;

            int count = await GetImageCount(msg);

            for (int t = 0; t < count; t++)
            {
                await PostImages(msg, im);
            }

            await Task.CompletedTask;

        }

        static async Task<int> GetImageCount(AbbybotCommandArgs msg)
        {
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
            return count;
        }

        static bool IsChannelNsfw(AbbybotCommandArgs msg)
        {
            bool isnsf = true;
            if (msg.channel is ITextChannel itc)
                isnsf = itc.IsNsfw;
            return isnsf;
        }

        static async Task<List<AbbybotUser>> GetMentionedUsers(AbbybotCommandArgs msg)
        {
            List<AbbybotUser> mentionedUsers = new List<AbbybotUser>();

            foreach (var u in msg.mentionedUserIds)
            {
                AbbybotUser au = null;
                if (u is SocketGuildUser sgu)
                {
                    au = await AbbybotUser.GetUserFromSocketGuildUser(sgu);
                }
                else
                {
                    au = await AbbybotUser.GetUserFromSocketUser(u);
                }
                mentionedUsers.Add(au);
            }

            return mentionedUsers;
        }

        async Task PostImages(AbbybotCommandArgs msg, ImgData im)
        {
            await GetImage(im);

            EmbedBuilder data = null;

            if (im.source == null)
                im.source = "no source";
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
                await msg.Send($"sorry master... I could not find the an {im.command.Replace("abbybot ", "")}ing picture for {fc}.. im sory");
                return;
            }

            if (msg.abbybotGuild != null)
            {
                bool comb = await IsNsfworLoli(msg, im, data);

                if (!comb)
                    await msg.Send(data);

            }
        }

        static async Task<bool> IsNsfworLoli(AbbybotCommandArgs msg, ImgData im, EmbedBuilder data)
        {
            bool loli = msg.abbybotGuild.NoLoli && im.loli;
            bool shot = msg.abbybotGuild.NoLoli && im.shot;
            bool nsfw = msg.abbybotGuild.NoNSFW && im.nsfw;
            bool comb = loli || shot || nsfw;
            if (comb)
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

                await msg.Send(reqj);
            }

            return comb;
        }

        async Task GetImage(ImgData im)
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


                im.loli = imgdata.Tags.Contains("loli");
                im.shot = imgdata.Tags.Contains("shot");
                im.nsfw = (imgdata.Rating == BooruSharp.Search.Post.Rating.Explicit || imgdata.Rating == BooruSharp.Search.Post.Rating.Explicit);
           

                if (imgdata.FileUrl != null)
                {
                    im.Imageurl = imgdata.FileUrl.ToString();
                }
                if (imgdata.Source != null)
                {
                    im.source = imgdata.Source;
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
            if (aca.abbybotUser.userTrust.inTimeOut && canrun)
            {
                await aca.Send($"You're in timeout for a little while. You did a mean thing and I can't stand for that. Check your time and details with abbybot timeout. Sorry.");
                canrun = false;
            }
            return canrun;
        }

        async Task<bool> CanRun(AbbybotCommandArgs md)
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
