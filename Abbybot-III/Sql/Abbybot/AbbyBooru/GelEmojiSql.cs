using AbbySql.Types;

using Discord;

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

			ulong abid = Apis.Discord.__client.CurrentUser.Id;
			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL($"SELECT * FROM abbybooru.emojis WHERE `AbbybotId`='{abid}' AND  `MessageId`='{messageId}' AND `Emoji`='{emoji}' AND `OwnerId`='{ownerId}';");



			return table.Count > 0 && table[0]["Type"] is int i ? i : -1;
			
		}

		public static async Task<string> GetEmojiCommand(ulong id, string name, ulong userId)
		{

			ulong abid = Apis.Discord.__client.CurrentUser.Id;
			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL($"SELECT GelbooruCommandJson FROM abbybooru.emojis WHERE `AbbybotId`='{abid}' AND  `MessageId`='{id}' AND `Emoji`='{name}' AND `OwnerId`='{userId}';");
			return table.Count > 0 && table[0]["GelbooruCommandJson"] is string i ? i : null;
		}

		public static async Task AddEmojiAsync(ulong oid, ulong mid, string t, int type, string msgjson, string resjson)
		{
			ulong abid = Apis.Discord.__client.CurrentUser.Id;
			string s = t.Contains(':') ? t.Split(":")[1] : t;  
			string emoji = AbbySql.AbbysqlClient.EscapeString(s);
			string jsoni = AbbySql.AbbysqlClient.EscapeString(msgjson);
			string jsoni2 = AbbySql.AbbysqlClient.EscapeString(resjson);
			await AbbySql.AbbysqlClient.RunSQL($"insert into abbybooru.emojis (`AbbybotId`,`OwnerId`, `MessageId`, `Emoji`, `Type`, `GelbooruCommandJson`, `GelbooruResultJson`) values ('{abid}','{oid}', '{mid}', '{emoji}', '{type}', '{jsoni}', '{jsoni2}');");
		}

		public static async Task AddEmojisAsync(ulong id1, ulong id2, (Emoji emoji, int type)[] es, string msgjson, string resjson)
		{
			foreach(var (e,t) in es) {
				await AddEmojiAsync(id1, id2, e.Name, t, msgjson, resjson);
			}
		}

		public static async Task<string> GetEmojiResult(ulong id, string name, ulong userId)
		{

			ulong abid = Apis.Discord.__client.CurrentUser.Id;

			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL($"SELECT GelbooruResultJson FROM abbybooru.emojis WHERE `AbbybotId`='{abid}' AND `MessageId`='{id}' AND `Emoji`='{name}' AND `OwnerId`='{userId}';");
			return table.Count > 0 && table[0]["GelbooruResultJson"] is string i ? i : null;
		}
	}
}
