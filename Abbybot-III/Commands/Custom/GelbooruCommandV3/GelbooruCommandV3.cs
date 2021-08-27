using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.User;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
	[Capi.Cmd("GelbooruCommandV3", 1, 1)]
	class GelbooruCommandV3 : BaseCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			PictureCommandSimplification pcI = new PictureCommandSimplification();
			if (aca.Contains(new string[] { "abbybot say", "abbybot whisper" })) return;
			PictureCommandData pcD = new PictureCommandData();
			pcD.user = aca.user;
		
			ulong guildId = 0, channelId = 0;

			if (aca.guild != null)
			{
				guildId = aca.guild.Id;
				channelId = aca.channel.Id;
			}
			var e = await aca.IncreasePassiveStat("GelCommandUsages");
			foreach (var sta in e)
			{
				pcD.index += sta.stat;
			}
			pcD.favoriteCharacter = aca.user.FavoriteCharacter;
			pcD.channelFavoriteCharacter = ((await ChannelFCOverrideSQL.GetFCMAsync(guildId, channelId)).fc is string sai && sai != "NO" ? sai : null);
			if (aca.isGuild)
			{
				pcD.isNSFW = aca.user.HasRatings(2) && aca.IsChannelNSFW && !aca.guild.NoNSFW;
				pcD.isLoli = aca.user.HasRatings(3) && !aca.guild.NoLoli;
				pcD.isGuildChannel = true;
			}
			pcD.mentions = await aca.GetMentionedUsers();
			pcD.ratings = aca.user.Ratings;
			pcD.message = aca.Message;
			pcD.badTags = (await UserBadTagListSql.GetbadtaglistTags(aca.user.Id)).ToArray();
			try
			{
				await aca.Send(await pcI.GetPicture(aca, pcD));
			}
			catch { }
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
		{
			return (await base.Evaluate(aca));
		}
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
    
	}
}