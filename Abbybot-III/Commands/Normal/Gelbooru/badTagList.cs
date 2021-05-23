using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot badtaglist", 1, 1)]
	class BadTagList : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs message)
		{
			StringBuilder FavoriteCharacter = new StringBuilder(message.Message.Replace(Command, ""));

			EmbedBuilder eb = new EmbedBuilder();
			if (FavoriteCharacter.Length > 0)
			{
				while (FavoriteCharacter[0] == ' ')
					FavoriteCharacter.Remove(0, 1);
				while (FavoriteCharacter[^1] == ' ')
					FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
			}
			if (FavoriteCharacter.Length < 1)
			{
				StringBuilder sb = new StringBuilder();
				eb.Title = $"here's your bad tag list";
				eb.Color = Color.Blue;
				foreach (var tag in await UserBadTagListSql.GetbadtaglistTags(message.user.Id))
					sb.AppendLine(tag);
				eb.Description = sb.ToString();

				await message.Send(eb);
				return;
			}
			var okis = FavoriteCharacter.ToString().Split(" ")[0];
			if (okis.Equals("add", StringComparison.InvariantCultureIgnoreCase))
			{
				FavoriteCharacter.Remove(0, 4);
				List<string> tags = new List<string>();
				FavoriteCharacter = FavoriteCharacter.Replace(" ", "_");
				string fc = FavoriteCharacter.ToString().ToLower();
				foreach (var item in fc.Replace("_and_", "&&").Replace(",", "&&").Split("&&"))
				{
					FavoriteCharacter.Clear().Append(item);
					while (FavoriteCharacter[0] == '_')
						FavoriteCharacter.Remove(0, 1);
					while (FavoriteCharacter[^1] == '_')
						FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
					tags.Add(FavoriteCharacter.ToString());
				}
				string reason = "";
				FavoriteCharacter.Clear();
				List<string> blt = new List<string>();
				List<string> failedblt = new List<string>();
				foreach (var item in tags)
				{
					try
					{
						try
						{
							BooruSharp.Search.Post.SearchResult imgdata = await AbbyBooru.Execute(new string[] { item });
						}
						catch
						{
							throw new Exception("This tag doesn't have images. I can't add it to your badtaglist.");
						}
						try
						{
							bool addedtag = await UserBadTagListSql.AddBadTag(message.user.Id, item);
							if (addedtag)
							{
								blt.Add(item);
								FavoriteCharacter.Append($"{item} ");
							}
						}
						catch
						{
							failedblt.Add(item);
						}
					}
					catch (Exception ecx)
					{
						reason = ecx.Message;
					}
				}

				var ff = new StringBuilder();
				if (failedblt.Count > 0)
				{
					for (int i = 0; i < failedblt.Count; i++)
					{
						string b = failedblt[i];
						ff.Append(b);
						if (i != failedblt.Count - 1)
						{
							ff.Append(", ");
						}
					}
				}
				StringBuilder fcmaa = new StringBuilder();
				if (blt.Count > 0)
				{
					eb.Title = $"{fc} Yayy!!";
					eb.Color = Color.Green;
					fcmaa.Append($"I added tags {FavoriteCharacter} to your gel bad tag list cutie {message.user.Preferedname} master!!");
					if (ff.Length > 0) fcmaa.Append(" (").Append(ff).Append(")");

					eb.Description = fcmaa.ToString();
				}
				else
				{
					eb.Title = reason;
					eb.Color = Color.Red;
					eb.ImageUrl = "https://cdn.discordapp.com/avatars/595308053448884294/69542a3eb0866c37f33aa63704fe3726.png";
					fcmaa.Append($"sorry {message.user.Preferedname} master...");
					fcmaa.Append(" I could not add these tags to your bad tag list...: ").Append(ff).Append("...");
					eb.Description = fcmaa.ToString();
				}

				await message.Send(eb);
				return;
			}
			else if (okis.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
			{
				FavoriteCharacter.Remove(0, 7);

				List<string> tags = new List<string>();
				FavoriteCharacter = FavoriteCharacter.Replace(" ", "_");
				string fc = FavoriteCharacter.ToString().ToLower();
				foreach (var item in fc.Replace("_and_", "&&").Replace(",", "&&").Split("&&"))
				{
					FavoriteCharacter.Clear().Append(item);
					while (FavoriteCharacter[0] == '_')
						FavoriteCharacter.Remove(0, 1);
					while (FavoriteCharacter[^1] == '_')
						FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
					tags.Add(FavoriteCharacter.ToString());
				}
				string reason = "";
				FavoriteCharacter.Clear();
				List<string> blt = new List<string>();
				List<string> failedblt = new List<string>();
				foreach (var item in tags)
				{
					try
					{
						bool addedtag = await UserBadTagListSql.UnbadtaglistTag(message.user.Id, item);
						if (addedtag)
						{
							blt.Add(item);
							FavoriteCharacter.Append($"{item} ");
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						failedblt.Add(item);
					}
				}
				var ff = new StringBuilder();
				if (failedblt.Count > 0)
				{
					for (int i = 0; i < failedblt.Count; i++)
					{
						string b = failedblt[i];
						ff.Append(b);
						if (i != failedblt.Count - 1)
						{
							ff.Append(", ");
						}
					}
				}
				StringBuilder fcmaa = new StringBuilder();
				if (blt.Count > 0)
				{
					eb.Title = $"{fc} Yayy!!";
					eb.Color = Color.Green;
					fcmaa.Append($"I removed tags {FavoriteCharacter} from your gel badtaglist cutie {message.user.Preferedname} master!!");
					if (ff.Length > 0) fcmaa.Append(" (").Append(ff).Append(")");

					eb.Description = fcmaa.ToString();
				}
				else
				{
					eb.Title = reason;
					eb.Color = Color.Red;
					eb.ImageUrl = "https://cdn.discordapp.com/avatars/595308053448884294/69542a3eb0866c37f33aa63704fe3726.png";
					fcmaa.Append($"sorry {message.user.Preferedname} master...");
					fcmaa.Append(" I could not remove these tags: ").Append(ff).Append("...");
					eb.Description = fcmaa.ToString();
				}

				await message.Send(eb);
			}
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return await Task.FromResult($"add tags you don't like to the bad tag list. Personally, i hate large breasts, but you do you.");
		}
	}
}