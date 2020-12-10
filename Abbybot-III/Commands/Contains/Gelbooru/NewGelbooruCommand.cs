using Abbybot_III.Apis.Discord.Events;
using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;

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
    class NewGelbooruCommand : ContainCommand
    {

        public override bool SelfRun
        {
            get => base.SelfRun = true;
            set => base.SelfRun = value;
        }

        List<string> tags = new List<string> { "" };

        public NewGelbooruCommand(string Command, string[] tags, CommandRatings Rating)
        {
            this.Rating = Rating;
            this.Command = Command;
            List<string> s = new List<string>();
            this.tags = tags.ToList();
        }

        public NewGelbooruCommand()
        {

        }

        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            if (aca.Message.Contains("%say") || aca.Message.Contains("%dm"))
                return;

            List<AbbybotUser> mentionedUsers = await aca.GetMentionedUsers();

            string fc = await GetFavoriteCharacterTagAsync(aca, mentionedUsers);
            List<string> tagz = await GenerateTags(aca, fc);
            
            var imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

            if (imgdata.Source == "noimagefound")
            {
                await NoImageFoundEmbed.Build(aca, fc);
                return;
            }

            bool loli = imgdata.Tags.Contains("loli");
            bool shot = imgdata.Tags.Contains("shota");
            bool nsfw = imgdata.Rating != BooruSharp.Search.Post.Rating.Safe;
            string fileurl = imgdata.FileUrl.ToString();
            string source = imgdata.Source;

            if (nsfw && !await aca.IsNSFW())
            {
                await aca.Send("master i can't search for nsfw pictures in a safe channel. there may be children.");
                return;
            }

            bool guildloli = false;
            bool guildnsfw = false;
            if (aca.abbybotGuild != null) {
                guildloli = aca.abbybotGuild.NoLoli;
                guildnsfw = aca.abbybotGuild.NoNSFW;
            }

            if ((loli&&guildloli) || (nsfw&&guildnsfw))
                await aca.Send("I can't send that to this server due to it opting not to allow nsfw.");
            else {

                ImgData imgdrata = new ImgData()
                {
                    command = Command,
                    Imageurl = fileurl,
                    loli = loli,
                    mentions = mentionedUsers,
                    nsfw = nsfw,
                    safe = !nsfw,
                    shot = shot,
                    source = source,
                    user = aca.abbybotUser,
                    sudouser = aca.abbybotSudoUser,
                    favoritecharacter = fc
                };

                var e = GelEmbed.Build(imgdrata);
                await aca.Send(e);
            }



        }

        private async Task<List<string>> GenerateTags(AbbybotCommandArgs aca, string fc)
        {
            var tagz = tags.ToList();

            
            tagz.Add($"{fc}*");

            if (!(aca.channel is SocketDMChannel sdc)) {
            if (!aca.abbybotUser.userPerms.Ratings.Contains((CommandRatings)2) || !await aca.IsNSFW())
            {
                tagz.Add("rating:safe");
            }
            if (!aca.abbybotUser.userPerms.Ratings.Contains((CommandRatings)3))
            {
                tagz.Add("-loli");
            }
            }

            var blacklisttags = await UserBlacklistSql.GetBlackListTags(aca.abbybotUser.Id);
            foreach (var item in blacklisttags)
            {
                tagz.Add($"-{item}");
            }

            return tagz;
        }

        private static async Task<string> GetFavoriteCharacterTagAsync(AbbybotCommandArgs aca, List<AbbybotUser> mentionedUsers)
        {
            
            if (aca.abbybotSudoUser != null)
                return "Abigail_Williams*";

            var ufcm = await FCMentionsSql.GetFCMAsync(aca.abbybotUser.Id);
            if (ufcm&&mentionedUsers.Count > 0)
                    return mentionedUsers.random().userFavoriteCharacter.FavoriteCharacter;
            else
                {
                    return aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter;
                }

            return "abigail_williams*";
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"These commands will show a picture of your favorite character ({aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter}) doing what's in the command. (for example: %hug has hugging inside it)";
        }
    }
}
