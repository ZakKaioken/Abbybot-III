using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;
using Discord;

namespace Abbybot_III.Commands.Normal.Gelbooru.FCSimplification
{
    [Capi.Cmd("abbybot fc", 1, 1)]
    class FCSimplification : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            var input = a.Replace(Command);

            if (input.Length == 0) 
                if (a.hasMultipleFcs)
                {
                    var fclists = GetMultiFCStringBuilders(a);

                    await a.Send(fclists);
                    return;
                } else {
                    string message = $"Your favorite character is: {a.BreakAbbybooruTag(a.user.FavoriteCharacter)}. Get help with {Command} help";

                    await a.Send(message);
                    return;
                }

            var ofclist = a.GetFCList();

            var subCommand = input.Split(" ")[0];
            var info = subCommand switch {
                "help" => await DoHelp(a),
                "history"=> await DoHistory(a),
                "undo"=> await DoUndo(a),
                "revert" => await DoRevert(a),
                "add" => await DoAdd(a),
                "remove" => await DoRemove(a),
                _ => await DoSet(a)
            };

            if (!info.cons) return;

            string[] o = new string[1];
            a.BuildAbbybooruTag(info.TitleFc);
			a.BuildAbbybooruTag(info.Fc);
            o[0] = (info.TitleFc ?? info.Fc).ToString(); 

            (bool canrun, Uri pictureurl, Uri previewurl, string fc) aaa = (false, null, null, "");

            int failcount = 0;
            bool failed = false;

            EmbedBuilder eb = new EmbedBuilder();
            
                bool set = false;
            do {
                var sif = o[0];
                try {
                    aaa = await awa(a, o, info.type);
                    if (!aaa.canrun)
                        throw new Exception("Can't run");
                } catch {
                    failcount++;
                    continue;
                }
                if (info.type == "set") {
                    info.Fc.Clear().Append(aaa.fc);
                o[0] = aaa.fc;
            }
                Console.WriteLine($"{sif}->{o[0]}");
                Uri[] uris = new Uri[] {
                    aaa.pictureurl, aaa.previewurl
                };
                int count = 0;
                foreach(var u in uris) {
                    try {
                        eb.ImageUrl = u.ToString();
                        set = true;
                        break;
                    } catch {
                        count++;
                    }
                }
                if (count >= uris.Length) {
                    failcount++;
                    failed = true;
                }

                Console.WriteLine($"set? {set}, fails?{failcount}, image? {eb.ImageUrl != null}");
            } while (!set&& failcount < 3);
            if (failed)
			{
				await a.Send("I'm sorry master... I tried really hard to find you a picture you would like... But nothing came up... I tried 3 searches... oh well...");
				return;
			}

            var oioio = a.BreakAbbybooruTag(((info.TitleFc !=null) ? info.TitleFc : info.Fc).ToString());
            Console.WriteLine($"{oioio}: {info.TitleFc}, {info.Fc}");
            Console.WriteLine(oioio);
            if (aaa.canrun) {
                var u = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
                await FavoriteCharacterSql.SetFavoriteCharacterAsync(a.user.Id, info.Fc.ToString());
                var lastId = (u.Count > 0 ? u[0].Id : 0);
                Console.WriteLine(lastId);
                await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(a.user.Id, info.Fc.ToString(), info.type, oioio, lastId);

                eb.Color = Color.Green;

                eb.Title = info.type switch {
                    "add" => $"Added {oioio}!!!",
                    "remove" => $"Removed {oioio}!!!",
                    "set" => $"{oioio} Yayy!!",
                    _ => "I'm kinda confused master..."
                };

                eb.Description = info.type switch {
                    "add" => $"I added {oioio} to your fc!!",
                    "remove" => $"I removed {oioio} from your fc!",
                    "set" => $"I know your favorite character now hehehehe cutie {a.user.Preferedname} master!! ",
                    _ => "I... I really don't know what's going on..."
                };
            } else {
                var foc = a.BreakAbbybooruTag (info.Fc.ToString());
                eb.Title = $"oof... {foc}...";
                eb.Description = $"sorry {a.user.Preferedname}... i couldn't find {foc} ({info.Fc}) ...";
                eb.Color = Color.Red;
            }

            await a.Send(eb);
            
            if (ofclist.Length > 2 && info.type == "set") await a.Send("Hey, ps... I think you may have made a mistake... You set that favorite character instead of adding it. \nIf you didn't mean to override your favorite character list, I suggest you to run: ``%fc undo``");
        }

        public static async Task<(bool canrun, Uri pictureurl, Uri previewurl, string fc)> awa(AbbybotCommandArgs aca, string[] o, string type)
		{
            
			string fc = o[0];
			Uri pictureurl = null;
			Uri previewurl = null;
			bool canrun = false; int tries = 0;
			do
			{
				try
				{
					BooruSharp.Search.Post.SearchResult imgdata = await aca.GetPicture(o);
                    Console.WriteLine("searched picture");
                    if (imgdata.Source.Contains("noimagefound"))
					{
                        Console.WriteLine("failed first test");
						throw new Exception("AAAA");

					}
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
							var i = await aca.GetPicture(e.ToArray());
							if (i.Source.Contains("noimagefound"))
								throw new Exception("AAAA");
							pictureurl = i.FileUrl;
							previewurl = i.PreviewUrl;
						}
						catch { Console.WriteLine("first fc test failed"); }

					}

					canrun = true;
                    Console.WriteLine($"{fc} can run");
					break;
				}
				catch (Exception e)
				{
					if (e.ToString().Contains("AAAA"))
					{
                        Console.WriteLine($"{o[0]} fail!ed...");

                        o[0] = aca.InvertName(o[0]);

                        fc = o[0];
					}
					else throw new Exception("failed");
				}
				tries++;
			} while (tries <= 4);
            Console.WriteLine($"ended with a fc of {fc}");
			return (canrun, pictureurl, previewurl, fc);
		}

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoHelp(AbbybotCommandArgs a)
        {
            
            EmbedBuilder eb = new EmbedBuilder();
            eb.AddField($"Set favorite character", $"``{Command} character name``", true);
            eb.AddField($"Add favorite character", $"``{Command} add character name``", true);
            eb.AddField($"Remove favorite character", $"``{Command} remove character name``", true);
            eb.AddField($"Favorite Character History", $"``{Command} history``", true);
            eb.AddField($"Revert Favorite Character", $"``{Command} revert 1``", true);
            eb.AddField($"Undo Character History Change", $"``{Command} undo``", true);
            eb.AddField($"Tips", $"using ``and`` will add an fc which has pictures containing those characters, ``or`` will separate fcs and give you a random fc every roll\nYou can use any gelbooru tag as an fc", false);
            eb.Color = Color.Orange;
            var efb = new EmbedFooterBuilder();
            efb.Text = "Abbybot";
            eb.Footer = efb;
            await a.Send(eb);
                    return ("help", false,null,null);
        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoHistory(AbbybotCommandArgs a)
        {
            var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
				var axis10 = axis.OrderByDescending(x => x.Id).Take(5);
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
                return ("history", false,null,null);
        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoUndo(AbbybotCommandArgs a)
        {
            Console.WriteLine("trying undo");
                var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
                var axis10 = axis.OrderByDescending(i=>i.Id).ToList();
				if (axis10.Count > 1)
				{
                Console.WriteLine("we can undo");
					var undoid = axis10[0].UndoId;
                    if (undoid == 0) return ("", false, null, null);
                    foreach(var i in axis10) {
                        Console.Write($"[{i.Id}]");
					}
                Console.WriteLine($"undoid is {undoid}");
                Console.WriteLine("undoid wasn't 0");
                var undo = axis10.Where(o => o.Id == undoid).ToList() ;
                if (undo.Count == 0) return ("", false, null, null);
                Console.WriteLine("there was an undo object");

                var nextundoid = undo[0].UndoId;
                if (nextundoid == 0) return ("", false, null, null);
                Console.WriteLine("the undo's id is not 0");
                await FavoriteCharacterSql.SetFavoriteCharacterAsync(a.user.Id, undo[0].FavoriteCharacter);
					await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(a.user.Id, undo[0].FavoriteCharacter, "undo", $"set back to ...{new string(undo[0].FavoriteCharacter.TakeLast(250).ToArray())}.", nextundoid);
					EmbedBuilder ebxz = new();
					ebxz.Title = "undo";
					ebxz.Description = $"Undone {new string(axis10[0].FavoriteCharacter.TakeLast(250).ToArray())}... was set back to ...{new string(undo[0].FavoriteCharacter.TakeLast(250).ToArray())}";

					await a.Send(ebxz);
				}
                return ("undo", false, null,null);
        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoRevert(AbbybotCommandArgs a)
        {
            var axis = await FavoriteCharacterHistorySql.GetFavoriteCharacterHistoryAsync(a.user.Id);
				var axis10 = axis.OrderByDescending(x => x.Id).ToList();
                var FavoriteCharacter = a.Replace(Command);
				FavoriteCharacter.Remove(0, 7);
				try
				{
					var iii = int.Parse(FavoriteCharacter.ToString());

					if (axis10.Count > 0)
					{
						var iaxisuf = axis10[iii].FavoriteCharacter;
						await FavoriteCharacterSql.SetFavoriteCharacterAsync(a.user.Id, iaxisuf);
						await FavoriteCharacterHistorySql.SetFavoriteCharacterHistoryAsync(a.user.Id, iaxisuf, "revert", $"reverted back to fc history: {axis10[iii].FavoriteCharacter}.", axis10[0].Id);
						EmbedBuilder ebxz = new();
						ebxz.Title = "reverted fc!";
						ebxz.Description = $"reverted {axis10[0].type} {axis10[0].Info}... back to {axis10[iii].type} {axis10[iii].Info}";

						await a.Send(ebxz);
						
					}
				}
				catch { }

                return ("revert", false, null, null);
        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoAdd(AbbybotCommandArgs a)
        {
                var FavoriteCharacter = a.Replace(Command);
                var TitleFC = a.Replace(Command);
				FavoriteCharacter.Remove(0, 4);
				TitleFC.Remove(0, 4);

			if ((FavoriteCharacter.Length <= 1)) return ("add", false, null, null);

			string[] o = new string[1] { a.BuildAbbybooruTag(FavoriteCharacter.ToString()) };
			var ava = await awa(a, o, "");
			FavoriteCharacter.Clear().Append(ava.fc);
            TitleFC.Clear().Append(ava.fc);
			a.BuildAbbybooruTag(FavoriteCharacter);
			foreach (var chr in a.GetFCList())
			{
				if (chr == "*" || chr == "**" || chr.Length <= 1) continue;
				if (chr.EndsWith("*"))
					FavoriteCharacter.Insert(0, $"{chr.Remove(chr.Length - 1)} or ");
				else FavoriteCharacter.Insert(0, $"{chr} or ");
			}
            while (FavoriteCharacter.Contains("__")) FavoriteCharacter.Replace("__", "_");

            Console.WriteLine($"{FavoriteCharacter},!!, {TitleFC}");
            return ("add", true, FavoriteCharacter, TitleFC);
        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoRemove(AbbybotCommandArgs a)
        {
            var FavoriteCharacter = a.Replace(Command);
            var TitleFC = a.Replace(Command);
            var fccx = a.GetFCList();
            FavoriteCharacter.Remove(0, 7);
				TitleFC.Remove(0, 7);
				a.BuildAbbybooruTag(FavoriteCharacter);
				if ((FavoriteCharacter.Length < 2)) return ("Remove", false, null,null);
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
                return ("Remove", true, FavoriteCharacter, TitleFC);

        }

        private async Task<(string type, bool cons, StringBuilder Fc, StringBuilder TitleFc)> DoSet(AbbybotCommandArgs a)
        {
            return ("set", true,a.Replace(Command),null); 
        }

        private static List<StringBuilder> GetMultiFCStringBuilders(AbbybotCommandArgs e)
        {
            var fcs = e.GetFCList().OrderBy(w=>w).ToArray();

            List<StringBuilder> sbs = new();
            StringBuilder currentSb = new($"You have {fcs.Length} favorite characters set!!!\n");
            foreach (var fc in fcs)
            {
                String fav = e.BreakAbbybooruTag(fc);
                if ((fav.Length+1 + currentSb.Length) >= 2000)
                {
                    sbs.Add(currentSb);
                    currentSb = new();
                }
                currentSb.Append(fav+'\n');
            }
            if (currentSb.Length > 0) sbs.Add(currentSb);

            return sbs;
        }
    }
}