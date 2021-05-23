using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler
{
	class CommandHandler
	{
		public static Capi.Command_Handler capi = new Capi.Command_Handler();

		public static async Task Handle(SocketMessage message)
		{
			AbbybotCommandArgs aca = await AbbybotCommandArgs.MakeArgsFromMessage(message);
			var osi = await UserTrustSql.GetUserTimeout(aca.user.Id);
			aca.user.inTimeOut = osi.inTimeout;
			aca.user.timeoutReason = osi.reason;
			aca.user.TimeOutEndDate = osi.timeoutEndDate;
			capi.Start(aca);
		}
	}
}