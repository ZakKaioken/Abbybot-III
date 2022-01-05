using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Events
{
	public class Components
	{
		public static void Init(DiscordSocketClient _client)
		{
			_client.ButtonExecuted += async (component) => await OnButtonUsed(component);
		}

		private static async Task OnButtonUsed(SocketMessageComponent smc)
		{
			if (smc.Data.CustomId == "EMOJI!") {
				await smc.RespondAsync($"you used the [{smc.Data.CustomId}] button!");
			}
		}
	}
}
