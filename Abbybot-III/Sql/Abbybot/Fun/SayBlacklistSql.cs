using AbbySql;
using AbbySql.Types;

using System.Collections.Generic;

using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Fun
{
	class SaybadtaglistSql
	{
		public static async Task<List<string>> GetbadtaglistTags()
		{
			List<string> tags = new List<string>();

			var table = await AbbysqlClient.FetchSQL("SELECT * FROM `saybadtaglist`");
			foreach (AbbyRow row in table)
			{
				tags.Add((row["Word"] is string favchan) ? favchan : "");
			}
			return tags;
		}
	}
}