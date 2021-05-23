using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.User;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
	[Capi.Cmd("GelbooruCommandV3", 1, 1)]
	class GelbooruCommandV3 : BaseCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			PictureCommandSimplification pcI = new PictureCommandSimplification();

			PictureCommandData pcD = new PictureCommandData();
			pcD.user = aca.user;
			ulong guildId = 0, channelId = 0;

			if (aca.guild != null)
			{
				guildId = aca.guild.Id;
				channelId = aca.channel.Id;
			}

			ulong abbybotId = Apis.Discord._client.CurrentUser.Id;
			await PassiveUserSql.IncreaseStat(abbybotId, guildId, channelId, aca.user.Id, "GelCommandUsages");
			var e = await PassiveUserSql.GetChannelsinGuildStats(abbybotId, guildId, aca.user.Id, "GelCommandUsages");
			foreach (var sta in e)
			{
				pcD.index += sta.stat;
			}
			pcD.favoriteCharacter = aca.user.FavoriteCharacter;
			pcD.channelFavoriteCharacter = (await ChannelFCOverride.GetFCMAsync(aca.guild.Id, aca.channel.Id) is string sai && sai != "NO" ? sai : null);
			if (aca.guild != null)
			{
				var ex = aca.user.Ratings.Contains((CommandRatings)2);
				pcD.isNSFW = aca.user.Ratings.Contains((CommandRatings)2) && (await aca.IsNSFW()) && !aca.guild.NoNSFW;
				pcD.isLoli = aca.user.Ratings.Contains((CommandRatings)3) && !aca.guild.NoLoli;
				pcD.isGuildChannel = true;
			}
			pcD.mentions = await aca.GetMentionedUsers();
			pcD.ratings = aca.user.Ratings;
			pcD.message = aca.Message;
			pcD.badTags = (await UserBadTagListSql.GetbadtaglistTags(aca.user.Id)).ToArray();
			try
			{
				await aca.Send(await pcI.GetPicture(pcD));
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