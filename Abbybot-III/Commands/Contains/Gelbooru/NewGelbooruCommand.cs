using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Commands.Normal.Gelbooru;
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

using static Google.Protobuf.WellKnownTypes.Field.Types;

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
			SearchResult imgdata = new();
			var ufc = "";

			var cfc = ("NO", false);
			if (aca.guild != null)
				cfc = await ChannelFCOverrideSQL.GetFCMAsync(aca.guild.Id, aca.channel.Id);

			bool found = false;
			int tries = 0;
			while (!found || tries < 5)
			{
				string fc = await GetFavoriteCharacterTagAsync(aca, mentionedUsers);
				if (fc.Contains(" ~ "))
			{
				string[] fcc = fc.Replace("{", "").Replace("}", "").Split(" ~ ");

				var e = await aca.IncreasePassiveStat("GelCommandUsages");

				foreach (var sta in e)
				{
					ix += sta.stat;
				}
				ix %= (ulong)fcc.Length;

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
				try
				{
					var o = await GetPicture(aca, fc, cfc.Item1, "abigail_williams*");
					imgdata = o.imgdata;
					ufc = o.ufc;
					found = true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					tries++;
				}
			}
			if (!found) {
				await aca.Send("I tried 6 different characters, as i could and still didn't find any pictures... ");
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
			if (aca.guild != null)
			{
				guildloli = aca.guild.NoLoli;
				guildnsfw = aca.guild.NoNSFW;
			}

			if ((loli && guildloli) || (nsfw && guildnsfw))
				await aca.Send("I can't send that to this server due to it opting not to allow nsfw.");
			else
			{
				var w = ufc.Replace("-abigail williams", "any waifu");
				
				ImgData imgdrata = new()
				{
					command = Command,
					Imageurl = fileurl,
					loli = loli,
					mentions = mentionedUsers,
					nsfw = nsfw,
					safe = !nsfw,
					shot = shot,
					source = source,
					user = aca.user,
					sudouser = aca.sudoUser,
					favoritecharacter = w
				};

				var e = GelEmbed.Build(aca, imgdrata, found, rolling);
				await aca.Send(e);
			}
		}

		async Task<(SearchResult imgdata, string ufc)> GetPicture(AbbybotCommandArgs aca, string fc, string cfc, string ufc) {
			(SearchResult imgdata, string ufc) picture = (new(), ufc);
			(string favoriteCharacter, string channelFavoriteCharacter)[] kinds = {
				(fc, cfc), (cfc,null), (fc, null), (ufc, null)
			};
			int index = 0;

			(List<string> tags, string fc, string cfc) set;
			do
			{
				try
				{
					set = await GenerateTags(aca, kinds[index]);
					picture.imgdata = await aca.GetPicture(set.tags.ToArray());
					if (picture.imgdata.Source == "noimagefound") 
						throw new Exception("CFC+FC FAILED");
					var cf = fc;
					if (set.cfc != null) 
						cf += $" and {set.cfc}";

					ufc = aca.BreakAbbybooruTag(cf);
				}
				catch (Exception e)
{
					Console.WriteLine(e);
					index++;
				}
			} while (index < kinds.Length);

			if (picture.imgdata.Source == "noimagefound")
				throw new Exception("noimagefound");
			return picture;
		}

		private async Task<(List<string> tags, string fc, string cfc)> GenerateTags(AbbybotCommandArgs aca, (string favoriteCharacter, string channelFavoriteCharacter) p)
		{
			(List<string> tags, string fc, string cfc) set = (tags.ToList(), p.favoriteCharacter, null);
			set.tags = tags.ToList();
			set.tags.Add($"{p.favoriteCharacter}*");
			if (p.channelFavoriteCharacter != null)
			{
				set.cfc = p.channelFavoriteCharacter;
				set.tags.Add($"{p.channelFavoriteCharacter}*");

			}
			if (aca.channel is not SocketDMChannel sdc)
			{
				if (!aca.user.HasRatings(2) || !aca.IsChannelNSFW) set.tags.Add("rating:safe");
				if (!aca.user.HasRatings(3)) set.tags.Add("-loli");
			}

			var badtaglisttags = (await UserBadTagListSql.GetbadtaglistTags(aca.user.Id));
			badtaglisttags.ForEach(tag => set.tags.Add($"-{tag}"));
			return set;
		}

		async Task<List<string>> GenerateTags(AbbybotCommandArgs aca, string fc, string cfc = "NO")
		{
			var tagz = tags.ToList();

			if (cfc != "NO")
				tagz.Add($"{cfc}*");
			tagz.Add($"{fc}*");

			if (aca.channel is not SocketDMChannel sdc)
			{
				if (!aca.user.HasRatings(2) || !aca.IsChannelNSFW) tagz.Add("rating:safe");
				if (!aca.user.HasRatings(3)) tagz.Add("-loli");
			}

			var badtaglisttags = await UserBadTagListSql.GetbadtaglistTags(aca.user.Id);
			foreach (var item in badtaglisttags)
			{
				tagz.Add($"-{item}");
			}

			return tagz;
		}

		static async Task<string> GetFavoriteCharacterTagAsync(AbbybotCommandArgs aca, List<AbbybotUser> mentionedUsers)
		{
			if (aca.sudoUser != null)
				return "Abigail_Williams*";
			
			var ufcm = await aca.GetFCMentions();
			if (ufcm && aca.isMentioning)
				return mentionedUsers.random().FavoriteCharacter;
			else
			{
				return aca.user.FavoriteCharacter;
			}
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			await Task.CompletedTask;
			return $"These commands will show a picture of your favorite character ({aca.user.FavoriteCharacter}) doing what's in the command. (for example: abbybot hug has hugging inside it)";
		}
	}
}