using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
	[Cmd("UsernameUpdate", 1, 1)]
	class UsernameUpdate : PassiveCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			await PassiveUserSql.SetUsernameSql(aca.user.Id, aca.user.Username, aca.user.Nickname);
		}
	}
}