using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.Guild.User;
using Abbybot_III.Sql.Abbybot.User;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks.Guild.User
{
	class MostActiveUserClock : RepeatingClock
	{
		public override async Task OnInit(DateTime time)
		{
			name = "user most active clock";
			delay = TimeSpan.FromMinutes(60);
			await base.OnInit(time);
		}

		public override async Task OnWork(DateTime time)
		{
			var roleIds = await MostActiveSql.GetRoles();
			var client = Apis.Discord._client;
			var abbybotid = client.CurrentUser.Id;

			foreach (var role in roleIds)
			{
				List<(ulong user, ulong role)> ugrs = new List<(ulong user, ulong role)>();
				var stats = await PassiveUserSql.GetGuildStat(abbybotid, role.guildId, "MessagesSent");
				var guildi = client.GetGuild(role.guildId);
				foreach (var sss in stats.ToList())
				{
					try
					{
						if (client.GetUser(sss.userId).IsBot) stats.Remove(sss);
					}
					catch
					{
						stats.Remove(sss);
					}
				}

				ulong count = 0;
				ulong score = 0;
				foreach (var user in stats)
				{
					count++;
					score += user.stat;
				}
				if (score == 0 || count == 0) continue;
				score = score / count;
				foreach (var user in stats.ToList())
				{
					if (user.stat < (score * .15f))
						stats.Remove(user);
				}

				stats = stats.OrderByDescending(x => x.stat).ToList();
				foreach (var user in stats)
				{
					var gxzer = await LastTimeSql.GetLastTime(user.userId, role.guildId, "Message");
					if (gxzer.Length > 2)

						if ((DateTime.Now - DateTime.Parse(gxzer)).TotalDays < 10)

							ugrs.Add((user.userId, role.roleId));
				}

				//get the a list of everyone who has sent a message at all in the last month
				var guz = client.GetGuild(role.guildId).Users.ToList();
				var rols = guildi.Roles.Where(x => x.Id == role.roleId).ToList();
				if (rols.Count > 0)
				{
					foreach (var gu in guz)
					{
						await Task.Delay(1000);

						if (ugrs.Select(x => x.user).Contains(gu.Id))
						{
							await gu.AddRoleAsync(rols[0]);
						}
						else
						{
							await gu.RemoveRoleAsync(rols[0]);
						}
					}
				}
			}

			//assign most active roll to the users who fit the category
		}
	}
}