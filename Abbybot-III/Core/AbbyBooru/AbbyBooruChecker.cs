﻿using Abbybot_III.Core.AbbyBooru.types;
using Abbybot_III.Core.Guilds.sql;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue.sql;
using Abbybot_III.Core.Twitter.Queue.types;
using Abbybot_III.Sql.AbbyBooru;
using Abbybot_III.Sql.Abbybot.Abbybot;



using Discord;
using Discord.WebSocket;

using Nano.XML;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru
{
    class AbbyBooruChecker
    {
        static DateTime CheckTime;

        public static void Init()
        {
            AbbyHeart.heartBeat += async (time) => await RequestBeat(time);
            CheckTime = DateTime.Now.AddMinutes(5);
        }

        static async Task RequestBeat(DateTime time)
        {
            if (time > CheckTime)
            {
                //Abbybot.print("running abc!!");
                CheckTime = CheckTime.AddMinutes(1);
                SocketTextChannel channel = null;
                List<Character> characters = await AbbyBooruCharacterSql.GetListFromSqlAsync();

                foreach (var character in characters)
                {
                    //Abbybot.print("{}");
                    //AbbybotGuild
                    bool safe = true;
                    List<string> tags = new List<string>();
                    try
                    {
                        var Gl = Apis.Discord.__client.Guilds.ToList().Any(x => x.Id == character.guildId);

                        if (!Gl)
                            continue;
                        //Console.WriteLine($"{character.tag}, guild found!");
                        var G = Apis.Discord.__client.GetGuild(character.guildId);

                        var pref = await GuildSql.GetGuild(character.guildId);
                        if (pref.PrefAbbybot != Apis.Discord.__client.CurrentUser.Id) 
                            continue;
                        channel = G.GetTextChannel(character.channelId);

                        tags.Add(character.tag);
                        //Console.WriteLine($"channel nsfw? {channel.IsNsfw}, character lewd? {character.IsLewd}");
                        safe = !channel.IsNsfw;
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e.ToString());
                        continue;
                    }
                    if (safe)
                        tags.Add("rating:safe");


                    Post[] charpicx = new Post[0];
                    try
                    {
                        charpicx = (await Apis.AbbyBooru.GetLatest(tags.ToArray())).ToArray();
                        if (charpicx == null) continue;
                        if (charpicx.Length > 5)
                            charpicx = charpicx.Take(5).ToArray();
                        else continue;
                    } catch {
                        //Console.WriteLine("aaaa");
					}
                    List<img> nngs = new List<img>();
                    var postIds = (await AbbyBooruCharacterSql.GetLatestPostIdsAsync(character));

                    Post[] eeee = charpicx.Where(x => !postIds.Contains((ulong)x.id)).Take(5).ToArray();

                    foreach (var ex in eeee)
                    {
                        img nng = new img()
                        {
                            Id = (ulong)ex.id,
                            imgurl = ex.fileUrl,
                            source = ex.source,
                            safe = (ex.rating.Contains($"safe")),
                            GelId = ex.id,
                            md5 = ex.md5
                        };
                        nngs.Add(nng);
                    }

                    for (int i = 0; i < nngs.Count; i++)
                    {
                        img sr = nngs[i];

                        EmbedBuilder embededodizer = new EmbedBuilder
                        {
                            ImageUrl = sr.imgurl
                        };

                        string fixedsource = "no source found";
                        try
                        {
                            if (sr.source != null)
                                fixedsource = sr.source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/");
                            
                            embededodizer.AddField($"New picture of {character.tag.Replace("_", " ")} :)", $"[Source]({fixedsource})");
                            embededodizer.Color = Color.LightOrange;

                            await channel.SendMessageAsync("", false, embededodizer.Build());
                        }
                        catch { }

                        await AbbyBooruCharacterSql.AddLatestPostIdAsync(character.Id, sr.Id, sr.GelId);

                        if (character.Id == 3)
						{
							var dl = await FunAbbybotFactsSql.GetFactsList(true, "twitter");
                            Random r = new Random();
                            var ranfac = dl.Count>0? dl[r.Next(0, dl.Count)].fact:"";

                            Tweet tweet = new()
                            {
                                message = $"A new tweet just came in from gelbooru!!\n{ranfac}",
                                url = sr.imgurl,
                                sourceurl = fixedsource,
                                GelId = sr.GelId
                            };

                            await TweetQueueSql.Add(tweet, true);
                        }
                    }
                }
            }
        }
    }
}