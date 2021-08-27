using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class FavoriteCharacterHistorySql
	{
		public static async Task SetFavoriteCharacterHistoryAsync(ulong userId, string favoriteCharacter, string type, string info, ulong lastId)
		{
			var t = DateTime.Now.ToString("G");
			var fc = AbbysqlClient.EscapeString(favoriteCharacter);
			var ty = AbbysqlClient.EscapeString(type);
			var inf = AbbysqlClient.EscapeString(info);
			await AbbysqlClient.RunSQL($"insert into `abbybooru`.`userfchistory` (`UserId`, `Time`, `FavoriteCharacter`, `Type`, `Info`, `UndoId`) values ('{userId}','{t}','{fc}', '{ty}', '{inf}', '{lastId}');");
		}

		public static async Task<List<(ulong Id, DateTime Time, string Info, string FavoriteCharacter, string type, ulong userId, ulong UndoId)>> GetFavoriteCharacterHistoryAsync(ulong userId)
		{
			List<(ulong Id, DateTime Time, string Info, string FavoriteCharacter, string type, ulong userId, ulong UndoId)> fch = new List<(ulong Id, DateTime Time, string Info, string FavoriteCharacter, string type, ulong userId, ulong UndoId)>();
			AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `abbybooru`.`userfchistory` where `UserId`='{userId}' order by `Id`desc limit 5 ");

			foreach (AbbyRow row in table)
			{
				Console.Write("id");
				ulong id = row["Id"] is ulong idid ? idid : 0;

				Console.Write("userid");
				ulong user = row["UserId"] is ulong uid ? uid : 0;

				Console.Write("userid");
				ulong UndoId = row["UndoId"] is ulong uunid ? uunid : 0;

				DateTime dt = new DateTime();

				Console.Write("time");
				try
				{
					string timestr = row["Time"] is string timest ? timest : "";
					dt = DateTime.Parse(timestr);
				}
				catch { }

				Console.Write("type");
				string typ = row["Type"] is string typest ? typest : "";

				Console.Write("FavoriteCharacter");
				string FC = row["FavoriteCharacter"] is string fc ? fc : "";

				Console.Write("info");
				string info = row["Info"] is string i ? i : "";

				Console.Write("^ADDING^");
				fch.Add((id, dt, info, FC, typ, user, UndoId));
			}

			return fch;
		}
	}
}