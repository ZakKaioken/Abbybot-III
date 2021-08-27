using Abbybot_III.Core.AbbyBooru.types;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.AbbyBooru
{
	class AbbyBooruCharacterSql
	{
		internal static async Task<List<Character>> GetListFromSqlAsync()
		{
			List<Character> c = new();
			var table = await AbbysqlClient.FetchSQL("SELECT * FROM `abbybooru`.`characters`");
			foreach (AbbyRow row in table)
			{
				ulong Id = row["Id"] is ulong u ? u : 0;
				bool IsLewd = row["IsLewd"] is int il && il > 0;
				string tag = row["Tag"] is string ta ? ta : "";
				ulong guildId = row["GuildId"] is ulong gid ? gid : 0;
				ulong channelId = row["ChannelId"] is ulong cid ? cid : 0;
				c.Add(new() { Id = Id, channelId = channelId, guildId = guildId, IsLewd = IsLewd, tag = tag });
			}
			return c;
		}

		internal static async Task<List<ulong>> GetLatestPostIdsAsync(Character character)
		{
			List<ulong> ulongs = new List<ulong>();
			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybooru`.`characterpostids` WHERE `CharId`='{character.Id}';");
			foreach (AbbyRow row in table) 
				if (row["Id"] is ulong id)
					ulongs.Add(id);
			return ulongs;
		}

		internal static async Task AddLatestPostIdAsync(ulong id1, ulong id2, int gelId)
		{
			await AbbysqlClient.RunSQL($"INSERT INTO `abbybooru`.`characterpostids` (`Id`, `CharId`, `GelId`) VALUES ('{id2}', '{id1}', '{gelId}');");
		}
	}
}
