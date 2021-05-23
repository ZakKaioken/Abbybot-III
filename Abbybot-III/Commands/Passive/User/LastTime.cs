using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Passive.User
{
	[Cmd("LastTime", 1, 1)]
	class LastTime : PassiveCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			var n = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			if (aca.guild != null)
			{
				await LastTimeSql.SetTimeSql(aca.user.Id, aca.guild.Id, "Message", n);
			}
			else
			{
				await LastTimeSql.SetTimeSql(aca.user.Id, 0, "Message", n);
			}
		}
	}
}