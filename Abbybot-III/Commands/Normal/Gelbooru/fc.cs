using Abbybot_III.Apis.Booru;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot fc", 1, 1)]
	class Fc : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			Console.WriteLine("starting fc");
			StringBuilder FavoriteCharacter = new StringBuilder(a.Message.ToLower().Replace(Command.ToLower(), ""));
			var zxxx = a.user.FavoriteCharacter;

			string[] fccx = zxxx.Replace("{", "").Replace("}", "").Split(" ~ ");

			if (FavoriteCharacter.Length < 1)
			{
				Console.WriteLine("nonuse fc check");
				Abbybot.print(zxxx.Contains(" ~ "));

				if (zxxx.Contains(" ~ "))
				{
					List<StringBuilder> sbs = new List<StringBuilder>();
					StringBuilder sb = new("you have ");
					sb.Append(fccx.Length).AppendLine(" favorite characters set!");

					StringBuilder CurrentItem = new StringBuilder(sb.ToString());
					foreach (var fcz in fccx)
					{
						if ((CurrentItem.Length + fcz.Length) >= 2000)
						{
							sbs.Add(new StringBuilder(CurrentItem.ToString()));
							CurrentItem.Clear();
						}
						CurrentItem.AppendLine(GelEmbed.fcbuilder(fcz));
					}
					if (CurrentItem.Length > 0)
						sbs.Add(new StringBuilder(CurrentItem.ToString()));
					CurrentItem.Clear();
					foreach (var sbzus in sbs)
					{
						await a.Send(sbzus.ToString());
						await Task.Delay(1000);
					}
				}
				else
				{
					var fcfc = GelEmbed.fcbuilder(zxxx);
					await a.Send($"Your favorite character is: {fcfc}. Get help with {Command} help");
				}

				return;
			}

			while (FavoriteCharacter[0] == ' ')
				FavoriteCharacter.Remove(0, 1);
			while (FavoriteCharacter.ToString().Contains("  "))
				FavoriteCharacter.Replace("  ", " ");
			while (FavoriteCharacter[^1] == ' ')
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

			Console.WriteLine("set up fc to be used");
			if (FavoriteCharacter.Length > 1)
			{
				string s = FavoriteCharacter.ToString();
				if (s == "help")
				{
					Console.WriteLine("help mode");
					await a.Send($"To set your favorite character use ``{Command} character name``\nTo remove the your favorite character do ``{Command} remove``\nYou can use the keywords **or** to randomly chose between multiple characters, **and** to have multiple characters in the same picture.\nyou can see what you set your fc to using ``{Command} history``\n``{Command} revert 2`` will revert the  character back to the 2nd fc change you've done recently\n``{Command} undo`` will undo the fc back to the last one you set!!\n**Tip:** You can use any gelbooru tag as the favorite character!");
					return;
				}
			}

			string fc = FavoriteCharacter.ToString();
			string ofc = fc;
			var TitleFC = new StringBuilder(ofc);
			bool addedtoexistingfc = false;
			bool removedfromexistingfc = false;
			var okis = fc.Split(" ")[0];
			string type = "set";
			if (okis.Equals("history", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("History Mode");
				var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
				var axis10 = axis.OrderByDescending(x => x.Id);
				StringBuilder sbib = new StringBuilder();
				sbib.AppendLine("FC History:");

				var list = axis10.ToList();
				for (int i = 0; i < list.Count; i++)
				{
					var axiz = list[i];
					sbib.Append(i).Append(". ").Append(axiz.type).Append(": ").Append(axiz.Info).Append("\n");
				}
				sbib.Append("Did you make a mistake master? You can undo a change to your FC like this: ``%fc undo``\nTo revert back to a specific FC type ``%fc revert 1`` for example.");

				await a.Send(sbib);
				return;
			}
			else if (okis.Equals("undo", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("undo mode");
				type = "undo";
				var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
				var axis10 = axis.OrderByDescending(x => x.Id).ToList();
				if (axis10.Count > 1)
				{
					var iaxisuf = axis10[1].FavoriteCharacter;
					await FavoriteCharacterSql.SetFavoriteCharacterAsync(a.user.Id, iaxisuf);
					await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(a.user.Id, iaxisuf, type, $"undo back to fc history {axis10[1].Id}.");
					EmbedBuilder ebxz = new EmbedBuilder();
					ebxz.Title = "undo";
					ebxz.Description = $"Undone {axis10[0].type} {axis10[0].Info}... back to {axis10[1].type} {axis10[1].Info}";

					await a.Send(ebxz);
					return;
				}
			}
			else if (okis.Equals("revert", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("revert mode");
				type = "revert";
				var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
				var axis10 = axis.OrderByDescending(x => x.Id).ToList();

				FavoriteCharacter.Remove(0, 7);
				try
				{
					var iii = int.Parse(FavoriteCharacter.ToString());

					if (axis10.Count > 0)
					{
						var iaxisuf = axis10[iii].FavoriteCharacter;
						await FavoriteCharacterSql.SetFavoriteCharacterAsync(a.user.Id, iaxisuf);
						await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(a.user.Id, iaxisuf, type, $"reverted back to fc history: {axis10[iii].FavoriteCharacter}.");
						EmbedBuilder ebxz = new EmbedBuilder();
						ebxz.Title = "reverted fc!";
						ebxz.Description = $"reverted {axis10[0].type} {axis10[0].Info}... back to {axis10[iii].type} {axis10[iii].Info}";

						await a.Send(ebxz);
						return;
					}
				}
				catch { }
			}
			else if (okis.Equals("add", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("add mode");
				type = "add";
				addedtoexistingfc = true;
				FavoriteCharacter.Remove(0, 4);
				TitleFC.Remove(0, 4);

				if ((FavoriteCharacter.Length <= 1)) return;

				foreach (var chr in fccx)
				{
					if (chr == "*" || chr == "**" || chr.Length <= 1) continue;
					if (chr.EndsWith("*"))
						FavoriteCharacter.Insert(0, $"{chr.Remove(chr.Length - 1)} or ");
					else FavoriteCharacter.Insert(0, $"{chr} or ");
				}
			}
			else if (okis.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("remove mode");
				type = "remove";
				FavoriteCharacter.Remove(0, 7);
				TitleFC.Remove(0, 7);
				FCBuilder(FavoriteCharacter);
				if ((FavoriteCharacter.Length < 2)) return;
				if (fccx.Contains(FavoriteCharacter.ToString()))
				{
					var eee = fccx.ToList();
					eee.Remove(FavoriteCharacter.ToString());
					FavoriteCharacter.Clear();
					if (eee.Count > 1)
					{
						foreach (var chr in eee)
						{
							if (chr == "*" || chr == "**" || chr.Length <= 1) continue;

							if (chr.EndsWith("*"))
								FavoriteCharacter.Insert(0, $"{chr.Remove(chr.Length - 1)} or ");
							else FavoriteCharacter.Insert(0, $"{chr} or ");
						}
					}
					else if (eee.Count == 1)
					{
						FavoriteCharacter.Append(eee[0]);
					}
				}

				removedfromexistingfc = true;
			}

			fc = FavoriteCharacter.ToString();
			FCBuilder(FavoriteCharacter);
			FCBuilder(TitleFC);
			if (FavoriteCharacter.Length <= 2) return;
			//string pictureurl = "https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg";
			var o = new string[1];
			var t = FavoriteCharacter.ToString();
			if (!addedtoexistingfc)
				o[0] = t;
			else
				o[0] = TitleFC.ToString();
			fc = t;

			EmbedBuilder eb = new EmbedBuilder();
			Console.WriteLine($"going to check the fc {o[0]}");
			int posi = 0; bool failed = false;
			(bool canrun, Uri pictureurl, Uri previewurl, string fc) aaa;
			do
			{
				aaa = await awa(o);
				fc = aaa.fc;
				try
				{
					Console.WriteLine("setting highest resolution picture as the preview...");
					eb.ImageUrl = aaa.pictureurl.AbsoluteUri;
					break;
				}
				catch
				{
					try
					{
						Console.WriteLine("setting highest resolution picture as the preview...");
						eb.ImageUrl = aaa.previewurl.AbsoluteUri;
						break;
					}
					catch
					{
						posi++;
						Console.WriteLine($"failed {posi} time");
						failed = true;
					}
				}
			}
			while (posi < 3);

			if (failed)
			{
				Console.WriteLine("position failed");
				await a.Send("I'm sorry master... I tried really hard to find you a picture you would like... But nothing came up... I tried 3 searches... oh well...");
				return;
			}
			Console.WriteLine("position passed");
			var u = a.user;
			if (aaa.canrun)
			{
				Console.WriteLine("making embeds");
				await FavoriteCharacterSql.SetFavoriteCharacterAsync(u.Id, t);
				var foc = GelEmbed.fcbuilder(fc);
				var oioio = (addedtoexistingfc || removedfromexistingfc) ? GelEmbed.fcbuilder(TitleFC.ToString()) : foc;
				await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(u.Id, t, type, oioio);

				eb.Color = Color.Green;

				if (addedtoexistingfc)
				{
					eb.Title = $"Added {oioio}!!!";
					eb.Description = $"I added {oioio} to your fc!!";
				}
				else if (removedfromexistingfc)
				{
					eb.Title = $"Removed {oioio}!!!";
					eb.Description = $"I removed {oioio} from your fc!";
				}
				else
				{
					eb.Title = $"{oioio} Yayy!!";
					eb.Description = $"I know your favorite character now hehehehe cutie {u.Preferedname} master!! ";
				}
			}
			else
			{
				var foc = GelEmbed.fcbuilder(fc);
				eb.Title = $"oof... {foc}...";
				eb.Color = Color.Red;
				eb.Description = $"sorry {u.Preferedname}... i couldn't find {foc} ({FavoriteCharacter}) ...";
			}
			Console.WriteLine("sending embeds");
			await a.Send(eb);
			if (fccx.Length > 2 && type == "set") await a.Send("Hey, ps... I think you may have made a mistake... You set that favorite character instead of adding it. \nIf you didn't mean to override your favorite character list, I suggest you to run: ``%fc undo``");
		}

		public static async Task<(bool canrun, Uri pictureurl, Uri previewurl, string fc)> awa(string[] o)
		{
			Uri pictureurl = null;
			string fc = o[0];
			Uri previewurl = null;
			bool canrun = false;
			int tries = 0;
			do
			{
				try
				{
					SearchResult imgdata = await AbbyBooru.Execute(o);
					if (imgdata.Source.Contains("noimagefound"))
						throw new Exception("AAAA");
					if (imgdata.Rating == BooruSharp.Search.Post.Rating.Safe)
					{
						pictureurl = imgdata.FileUrl;
						previewurl = imgdata.PreviewUrl;
					}
					else
					{
						var e = o.ToList();
						e.Add(" rating:safe ");
						try
						{
							var i = await AbbyBooru.Execute(e.ToArray());
							if (i.Source.Contains("noimagefound"))
								throw new Exception("AAAA");
							pictureurl = i.FileUrl;
							previewurl = i.PreviewUrl;
						}
						catch { }
					}

					canrun = true;
					break;
				}
				catch (Exception e)
				{
					if (e.ToString().Contains("AAAA"))
					{
						o[0] = InvertName(o[0]);
						fc = o[0];
					}
					else throw new Exception("failed");
				}
				tries++;
			} while (tries <= 4);

			return (canrun, pictureurl, previewurl, fc);
		}

		public static StringBuilder FCBuilder(StringBuilder FavoriteCharacter)
		{
			FavoriteCharacter.Replace(" ", "_").Replace("abbybot", "abigail_williams").Replace("abby_kaioken", "abigail_williams");
			if (FavoriteCharacter[^1] != '~')
				FavoriteCharacter.Append("*");
			else
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

			if (FavoriteCharacter.ToString().Contains("_~_") || FavoriteCharacter.ToString().Contains("_or_"))
			{
				FavoriteCharacter.Insert(0, "{").Append("}");
				FavoriteCharacter.Replace("~_or_", " ~ ").Replace("~_~_", " ~ ").Replace("_~_", "* ~ ").Replace("_or_", "* ~ ");
			}
			FavoriteCharacter.Replace("~_&&_", " ").Replace("~_and_", " ").Replace("_&&_", "* ").Replace("_and_", "* ");
			return FavoriteCharacter;
		}

		static string InvertName(string sbsb)
		{
			var sb = new StringBuilder();
			var orchars = sbsb.Split(" ~ ");
			foreach (var orchar in orchars)
			{
				var andchars = orchar.Replace("{", "").Replace("}", "").Split(" ");
				foreach (var andchar in andchars)
				{
					var subnames = andchar.Replace("*", "").Split("_");
					if (subnames.Length == 1)
					{
						sb.Append(subnames[0]);
					}
					if (subnames.Length == 2)
					{
						sb.Append($"{subnames[^1]}_{subnames[0]}*");
					}
					else if (subnames.Length == 3)
					{
						sb.Append($"{subnames[^1]}_{subnames[1]}_{subnames[0]}*");
					}
					if (andchars.Length > 1)
						sb.Append(" ");
				}
				if (orchars.Length > 1)
				{
					sb.Append(" ~ ");
				}
			}
			if (orchars.Length > 1)
			{
				sb.Insert(0, "{");
				sb.Append("}");
			}
			var s = sb.ToString();
			return s;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"set your favorite character. (all pictures are of your favorite character)";
		}
	}
}