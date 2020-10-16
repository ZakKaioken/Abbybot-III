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

            string fc = await GetFavoriteCharacterTag(aca, mentionedUsers);
            List<string> tagz = await GenerateTags(aca, fc);
            
            var imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

            if (imgdata.source == "noimagefound")
            {
                await NoImageFoundEmbed.Build(aca, fc);
                return;
            }

            bool loli = imgdata.tags.Contains("loli");
            bool shot = imgdata.tags.Contains("shota");
            bool nsfw = imgdata.rating != BooruSharp.Search.Post.Rating.Safe;
            string fileurl = imgdata.fileUrl.ToString();
            string source = imgdata.source;

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

        private static async Task<string> GetFavoriteCharacterTag(AbbybotCommandArgs aca, List<AbbybotUser> mentionedUsers)
        {
            string fc;
            if (aca.abbybotSudoUser != null)
            {
                fc = "Abigail_Williams*";
            }
            else
            {
                
                if (mentionedUsers.Count > 0)
                {
                    var user = mentionedUsers.random();
                    fc = user.userFavoriteCharacter.FavoriteCharacter;
                }
                else
                {
                    fc = aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter;
                    if (fc.Length < 2)
                        fc = "abigail_williams*";
                }
            }

            return fc;
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"{Command} shows a picture of {aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter} {Command.Replace("%", "")}ing someone!";
        }
    }
}
