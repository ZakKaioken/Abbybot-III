using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using BooruSharp.Search.Post;
using Capi.Interfaces;


namespace Abbybot_III.Commands.Contains.GelbooruV4
{
public class GelbooruCommand {
    public string[] tags;
    public int rating = -1;
    public string command = "missing", nickname="";
    public List<(string, string)> types;
    public Message message;

    public async Task<GelbooruResult> GenerateAsync(AbbybotCommandArgs aca) {
        
				int index = 0;
				var e = await aca.IncreasePassiveStat("GelCommandUsages");
				foreach(var i in e) 
					index += (int)i.stat;
				message.pfc = GetRandomFC(index);
				if (TestCommand()) 
                    return null;
                    
				GenTypes(message.pfc);
				var tags = AddBadTags();

				List<GelbooruResult> results;
				index = 0;
				do {
				var ee = AddTags(tags, index);
				message.ufc = ee.usedfc;
				results = await GetPictureAsync(ee.tagz.ToArray());
				} while (results == null  && ++index < types.Count);

				if (results == null||results.Count == 0)
				{
					await aca.Send($"Master... I didn't find a {nickname}ing picture of {message.pfc}");
					return null;
				}

				if (!message.isNSFW && results[0].Nsfw)
				{
					await aca.Send("Master that's a lewd image... I can't send it...");
					return null;
				}

				if (!message.isLoli && results[0].ContainsLoli)
				{
					await aca.Send("Master... I found an image, but it's against discord's tos so i'm not going to send it.");
					return null;
				}
        return results[0];
    }

    public async Task<List<GelbooruResult>> GetPictureAsync(string[] tags) {
        return await Abbybot_III.Apis.AbbyBooru.ExecuteAsync(tags);
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
        if (types == null) {
            types = new() {
                (message.cfc, pfc),
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
        return message.fcs[message.index++];
    }



}
}