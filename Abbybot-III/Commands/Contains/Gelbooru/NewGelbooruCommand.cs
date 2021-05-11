using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.User;

using BooruSharp.Search.Post;

using Capi.Interfaces;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
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

		readonly List<string> tags = new List<string> { "" };

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
			if (aca.Message.Contains("abbybot say") || aca.Message.Contains("abbybot whisper"))
				return;

			bool rolling = true; ulong ix = 0;

			List<AbbybotUser> mentionedUsers = await aca.GetMentionedUsers();

			var cfc = "NO";
			if (aca.abbybotGuild != null)
				cfc = await ChannelFCOverride.GetFCMAsync(aca.abbybotGuild.GuildId, aca.channel.Id);
			string fc = await GetFavoriteCharacterTagAsync(aca, mentionedUsers);

			if (fc.Contains(" ~ "))
			{
				string[] fcc = fc.Replace("{", "").Replace("}", "").Split(" ~ ");

				ulong guildId = 0, channelId = 0;

				if (aca.abbybotGuild != null)
				{
					guildId = aca.abbybotGuild.GuildId;
					channelId = aca.channel.Id;
				}

				ulong abbybotId = Apis.Discord.Discord._client.CurrentUser.Id;
				await PassiveUserSql.IncreaseStat(abbybotId, guildId, channelId, aca.abbybotUser.Id, "GelCommandUsages");
				var e = await PassiveUserSql.GetChannelsinGuildStats(abbybotId, guildId, aca.abbybotUser.Id, "GelCommandUsages");

				foreach (var sta in e)
				{
					ix += sta.stat;
				}
				ix = ix % (ulong)fcc.Length;
				if (!rolling)
				{
					Random r = new Random();
					fc = fcc[r.Next(fcc.Length)];
				}
				else
				{
					fc = fcc[ix];
				}
			}

			List<string> tagz;
			SearchResult imgdata;
			bool found = true;
			var ufc = "abigail_williams*";
			try
			{
				if (cfc == "NO") throw new Exception("cfc not set");

				tagz = await GenerateTags(aca, fc, cfc);

				imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

				if (imgdata.Source == "noimagefound")
				{
					throw new Exception("CFC+FC FAILED");
				}
				ufc = GelEmbed.fcbuilder($"{fc} {cfc}");
			}
			catch
			{
				try
				{
					if (cfc == "NO") throw new Exception("cfc not set");
					tagz = await GenerateTags(aca, cfc);

					imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

					if (imgdata.Source == "noimagefound")
					{
						throw new Exception("CFC FAILED");
						//ufc = GelEmbed.fcbuilder($"{cfc}");
					}
				}
				catch
				{
					try
					{
						tagz = await GenerateTags(aca, fc);

						imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

						if (imgdata.Source == "noimagefound")
						{
							throw new Exception("FC FAILED");
						}
						ufc = GelEmbed.fcbuilder($"{fc}");
					}
					catch
					{
						tagz = await GenerateTags(aca, ufc);
						found = false;
						imgdata = await Apis.Booru.AbbyBooru.Execute(tagz.ToArray());

						if (imgdata.Source == "noimagefound")
						{
							throw new Exception("FC FAILED");
						}
					}
				}
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
			if (aca.abbybotGuild != null)
			{
				guildloli = aca.abbybotGuild.NoLoli;
				guildnsfw = aca.abbybotGuild.NoNSFW;
			}

			if ((loli && guildloli) || (nsfw && guildnsfw))
				await aca.Send("I can't send that to this server due to it opting not to allow nsfw.");
			else
			{
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

				var e = GelEmbed.Build(imgdrata, found, rolling);
				await aca.Send(e);
			}
		}

		async Task<List<string>> GenerateTags(AbbybotCommandArgs aca, string fc, string cfc = "NO")
		{
			var tagz = tags.ToList();

			if (cfc != "NO")
				tagz.Add($"{cfc}*");
			tagz.Add($"{fc}*");

			if (aca.channel is not SocketDMChannel sdc)
			{
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

		static async Task<string> GetFavoriteCharacterTagAsync(AbbybotCommandArgs aca, List<AbbybotUser> mentionedUsers)
		{
			if (aca.abbybotSudoUser != null)
				return "Abigail_Williams*";

			var ufcm = await FCMentionsSql.GetFCMAsync(aca.abbybotUser.Id);
			if (ufcm && mentionedUsers.Count > 0)
				return mentionedUsers.random().userFavoriteCharacter.FavoriteCharacter;
			else
			{
				return aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter;
			}
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			await Task.CompletedTask;
			return $"These commands will show a picture of your favorite character ({aca.abbybotUser.userFavoriteCharacter.FavoriteCharacter}) doing what's in the command. (for example: abbybot hug has hugging inside it)";
		}
	}
}