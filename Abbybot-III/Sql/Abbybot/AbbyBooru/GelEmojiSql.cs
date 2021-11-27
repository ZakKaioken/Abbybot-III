using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.AbbyBooru
{
	class GelEmojiSql
	{
		public static async Task<int> GetEmojiType(ulong messageId, string emoji, ulong ownerId) {
			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL($"SELECT * FROM abbybooru.emojis WHERE `MessageId`='{messageId}' AND `Emoji`='{emoji}' AND `OwnerId`='{ownerId}';");
			
			return table.Count > 0&&table[0]["Type"] is int i ? i : -1;
		}

		public static async Task<string> GetEmojiCommand(ulong id, string name, ulong userId)
		{
			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL($"SELECT * FROM abbybooru.emojis WHERE `MessageId`='{id}' AND `Emoji`='{name}' AND `OwnerId`='{userId}';");
			return table.Count > 0 && table[0]["GelbooruCommandJson"] is string i ? i : null;
		}
	}
}
