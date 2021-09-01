using Abbybot_III.Core.Abbybot;

using AbbySql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Fun
{
	public class LuaSql
	{
		public static async Task<LuaData> GetLuaData(ulong UserId) 
		{
			LuaData lua = new() { UserId = UserId };
			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM user.lua where UserId = {UserId} ORDER BY Id ASC");
			table.ToList().
				ForEach(r =>
				{
					if (r["lua"] is string s) 
						lua.LuaPieces.Add(s); 
				});
			return lua;
		}

		public static async Task AddLuaData(ulong UserId, string piece) 
		{
			piece = AbbysqlClient.EscapeString(piece);
			await AbbysqlClient.RunSQL($"insert into user.lua (`UserId`, `lua`) values ('{UserId}', '{piece}');");
		}

		public static async Task ClearLuaData(ulong UserId) 
		{
			await AbbysqlClient.RunSQL($"delete from user.lua where `UserId` = '{UserId}';");
		}
	}
}
