using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.AbbyBooru;
using Abbybot_III.Sql.Abbybot.User;


using Capi.Interfaces;

using Discord;
using Discord.Rest;

using Nano.XML;

namespace Abbybot_III.Commands.Contains.GelbooruV4
{
public class GelbooruCommand {
    public string[] tags;
    public int rating = -1;
    public string command = "missing", nickname="";
    public List<(string, string)> types;
    public Message message;

    public async Task<Post> GenerateAsync(Action<string> OnFail = null) {
        
				int index = 0;
            List<(ulong channel, ulong stat)> osi = null;
            string stat = "GelCommandUsages";
            try
            {
                await PassiveUserSql.IncreaseStat(message.abbybotId, message.guildId, message.channelId, message.userId, stat);
                osi = await PassiveUserSql.GetChannelsinGuildStats(message.abbybotId, message.guildId, message.userId, stat);
            } catch { }

				foreach(var i in osi) 
					index += (int)i.stat;
				message.pfc = GetRandomFC(index);
				if (TestCommand()) 
                    return null;
                    
				GenTypes(message.pfc);
				var tags = AddBadTags();

				List<Post> results;
				index = 0;
				do {
				var ee = AddTags(tags, index);
				message.ufc = ee.usedfc;
				results = await GetPictureAsync(ee.tagz.ToArray());
				} while (results == null  && ++index < types.Count);

				if (results == null||results.Count == 0)
				{
					OnFail?.Invoke($"Master... I didn't find a {nickname}ing picture of {message.pfc}");
					return null;
				}

				if (!message.isNSFW && results[0].Nsfw)
				{
                    OnFail?.Invoke("Master that's a lewd image... I can't send it...");
					return null;
				}

				if (!message.isLoli && results[0].Loli)
				{
                    OnFail?.Invoke("Master... I found an image, but it's against discord's tos so i'm not going to send it.");
					return null;
				}
        return results[0];
    }

        //we need a better way to add multiple emojis to the db lol
		public async Task AddReactionsAsync(RestUserMessage abm, Post gelbooruResult)
		{
            var msgjson = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            var resjson = Newtonsoft.Json.JsonConvert.SerializeObject(gelbooruResult);
            (Emoji emoji, int type)[] e = new(Emoji, int)[]
            {
                (new("a:abbybongo:728506581423095838"), 0),
                (new("❌"), 2),
                (new("⚙️"), 1),
                (new("😵‍💫"), 3)
             };
            await abm.AddReactionsAsync(e.Select(e=>e.emoji).ToArray());
            await GelEmojiSql.AddEmojisAsync(message.user.Id, abm.Id, e, msgjson, resjson);
        }

		public async Task<List<Post>> GetPictureAsync(string[] tags) {
            return await Apis.AbbyBooru.GetRandomPost( tags );
    }

    public (List<string> tagz, string usedfc) AddTags(List<string> tags, int index)
	{
		var tagz = tags.ToList();
		string ww = "";
		if (types[index].Item1 is string i1)
		{
			tagz.Add(i1);
			ww += i1;
		}
		if (types[index].Item2 is string i2)
			tagz.Add(i2);
		return (tagz, ww);
	}
    public List<string> AddBadTags() {
        List<string> tagd = tags.ToList(); //make a list using the command tags split by space
        if (message.isGuild) //if this is not a dms channel
        {
            //remove all nsfw if we can't show it
            if (!message.isNSFW)
                tagd.Add("rating:safe");
            //remove all loli/shota content if we can't show it
            if (!message.isLoli)
            {
                tagd.Add("-loli");
                tagd.Add("-shota");
            }
        }
        //remove all content that the user/server doesn't like/allow
        foreach (var badTag in message.badTags)
            tagd.Add($"-{badTag}");
        return tagd;
    }
    
    public void GenTypes(string pfc) {
        
            types = new() {
                (pfc, message.cfc),
                (message.cfc, null)
            };
            if (!(message.cfc != null && message.cfc.Length! > 0))
            {
                types.AddRange(new (string, string)[]
                {
                    (pfc, null), 
                    ("abigail_williams*", null)
                });
            }
        
    }

    //this feels wrong
    public bool TestCommand() {
        bool run = true;
        run &= !(tags == null || rating==-1);
        run &= !(!message.ratings.Contains((CommandRatings)rating));
        run &= !(!message.isNSFW && 2==rating);
        run &= !(!message.isLoli && 3==rating);  
        return !run;
    }

    public string GetRandomFC(int stat) {
        if (message == null) return "";
        message.index = (stat+message.index)%message.fcs.Count;
        return message.fcs[message.index];
    }



}
}