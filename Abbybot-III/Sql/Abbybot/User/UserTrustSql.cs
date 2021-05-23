using Abbybot_III.Core.Data.User;

using AbbySql;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class UserTrustSql
	{
		public static async Task<(bool inTimeout, DateTime timeoutEndDate, string reason)> GetUserTimeout(ulong auId)
		{
			var abisb = new StringBuilder();
			abisb.Append($"SELECT * FROM `usertimeout` WHERE `UserId` = '{auId}';");
			var table = await AbbysqlClient.FetchSQL(abisb.ToString());
			(bool t, DateTime ted, string r) vars = (false, DateTime.Now, "");
			if (table.Count > 0)
			{
				vars.t = true;
				vars.ted = (table[0]["Time"] is string s) ? DateTime.Parse(s) : DateTime.Now;
				vars.r = (table[0]["Reason"] is string z) ? z : "";
			}

			if (vars.ted < DateTime.Now)
			{
				await AbbysqlClient.RunSQL($"DELETE FROM `usertimeout` WHERE `UserId` = '{auId}'");
				vars.t = false;
			}

			return vars;
		}
	}
}