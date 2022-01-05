using AbbySql;

using Discord.Rest;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Guild
{
    class InviteCounterSql
    {
        public static async Task AddGuild(SocketGuild guild, RestInviteMetadata rim)
        {
            await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`invites`(GuildId, InviteId, Joins) VALUES ('{guild.Id}', '{rim.Code}', '{rim.Uses}'); ");
        }

        public static async Task UpdateGuildName(SocketGuild guild, RestInviteMetadata rim)
        {
            var name = AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`invites` SET `Joins` = '{rim.Uses}' WHERE `GuildId` ='{guild.Id}' AND `InviteId` = '{rim.Code}';");
        }

        public static async Task<int> GetGuild(SocketGuild abg, RestInviteMetadata rim)
        {
            var table = await AbbysqlClient.FetchSQL($"SELECT `Joins` FROM `invites` WHERE `GuildId` = '{abg.Id}' AND `InviteId` = '{rim.Code}';");

            bool e = table.Count > 0;
			return e ? (int)table[0]["Joins"] : 0;
		}
	}
}