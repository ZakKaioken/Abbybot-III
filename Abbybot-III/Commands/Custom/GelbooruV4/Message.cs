using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;
using Capi.Interfaces;

public class Message {
    public string message;
	public List<CommandRatings> ratings=new();
    public List<string> fcs=new(), badTags=new();
    public List<AbbybotUser> mentions = new List<AbbybotUser>();
	
    public string cfc, pfc, ufc;
    public bool isNSFW, isLoli, isGuild;
    public int index=0;
    public AbbybotUser user;
    public ulong guildId=0, channelId=0, userId=0;
    
    public async Task Init(AbbybotCommandArgs aca) {
        if (aca.Contains(new string[] { "abbybot say", "abbybot whisper" })) return;
			user = aca.user;
			if (aca.guild != null)
			{
				guildId = aca.guild.Id;
				channelId = aca.channel.Id;
			}
			fcs = aca.GetFCList().ToList();
			cfc = ((await ChannelFCOverrideSQL.GetFCMAsync(guildId, channelId)).fc is string sai && sai != "NO" ? sai : null);
			if (aca.isGuild)
			{
				isNSFW = aca.user.HasRatings(2) && aca.IsChannelNSFW && !aca.guild.NoNSFW;
				isLoli = aca.user.HasRatings(3) && !aca.guild.NoLoli;
				isGuild = true;
			}
			mentions = await aca.GetMentionedUsers();
			ratings = aca.user.Ratings;
			message = aca.Message;
			badTags = (await UserBadTagListSql.GetbadtaglistTags(aca.user.Id)).ToList();

    }
    
    public Message () {
        
    }
}