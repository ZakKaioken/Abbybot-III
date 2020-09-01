using Abbybot_III.Apis.Twitter.Core;
using Abbybot_III.Core.AbbyBooru.types;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue.types;

using BooruSharp.Search.Post;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru
{
    class AbbyBooruChecker
    {
        static DateTime CheckTime;
        public static void Init()
        {
            AbbyHeart.heartBeat += async (time) => await RequestBeat(time);
            CheckTime = DateTime.Now.AddSeconds(10);
        }

        private static async Task RequestBeat(DateTime time)
        {
            if (time > CheckTime)
            {
                CheckTime = CheckTime.AddMinutes(1);
                SocketTextChannel channel = null;
              List <Character> characters = await Character.GetListFromSqlAsync();

                foreach(var character in characters) {
                    bool safe = true;
                    List<string> tags = new List<string>();
                    try {
                        tags.Add(character.tag);
                      channel = (SocketTextChannel)Apis.Discord.Discord._client.GetGuild(character.guildId).GetChannel(character.channelId);
                     safe = !channel.IsNsfw || !character.IsLewd;
                    }
                    catch
                    {

                        continue;
                    }
                    if (safe)
                        tags.Add("rating:safe");

                    SearchResult[] charpicx = (await Apis.Booru.AbbyBooru.GetLatest(tags.ToArray())).Take(5).ToArray();

                    List<img> nngs = new List<img>();
                    var postIds = (await Character.GetLatestPostIdsAsync(character));

                    SearchResult[] eeee = charpicx.Where(x => !postIds.Contains((ulong)x.id)).Take(5).ToArray();

                    foreach (var ex in eeee)
                    {
                        img nng = new img() { 
                            Id = (ulong)ex.id, 
                            imgurl = ex.fileUrl.ToString(), 
                            source = ex.source, 
                            safe = (ex.rating== Rating.Safe) 
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
                            string fixedsource = sr.source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/");
                            embededodizer.AddField($"New picture of {character.tag.Replace("_", " ")} :)", $"[Source]({fixedsource})");
                            embededodizer.Color = Color.LightOrange;

                        try {   
                            await channel.SendMessageAsync("", false, embededodizer.Build());
                        } catch { }

                        if (character.Id == 3)
                        {
                            Tweet tweet = new Tweet()
                            {
                                message = "A new tweet just came in from gelbooru!!",
                                url = sr.imgurl,
                                sourceurl = fixedsource
                            };
                            await TweetSender.SendTweet(tweet);
                        }


                        await Character.AddLatestPostIdAsync(character.Id, sr.Id);

                    }

                }
        }
    }
    }
}
