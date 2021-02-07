using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Mysql;

using AbbySql;
using AbbySql.Types;

using Capi.Interfaces;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Roles.sql
{
    public class RoleSql
    {
        public static async Task<bool> SetRole(ulong user, ulong guild, ulong role)
        {
            bool dre = await DoesRoleExist(user, guild, role);

            if (dre)
                return false;

            StringBuilder abisb = new StringBuilder();
            abisb.Clear();
            abisb.Append("INSERT INTO `discord`.`roles` (`UserId`,`GuildId`,`RoleId`) ");
            abisb.Append("VALUES ");
            abisb.Append($"('{user}', ");
            abisb.Append($"'{guild}',");
            abisb.Append($"'{role}'); ");

            bool ran = true;
            try
            {
                await AbbysqlClient.RunSQL(abisb.ToString());
            }
            catch (Exception e)
            {
                Abbybot_III.Abbybot.print(e.Message + e.StackTrace);
            }
            return ran;
        }

        public static async Task<List<AbbybotRole>> GetRoles(SocketGuild g)
        {
            StringBuilder abisb = new StringBuilder();
            abisb.Clear();
            abisb.Append("SELECT * FROM `discord`.`guildroles` WHERE `GuildId` = '");
            abisb.Append(g.Id);
            abisb.Append("';");

            List<AbbybotRole> roles = new List<AbbybotRole>();
            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            foreach (AbbyRow row in table)
            {
                ulong Roleid = (ulong)(row["Id"]);
                int Ranking = (int)row["Ranking"];
                roles.Add(new AbbybotRole((ulong)Roleid, new CommandRatings[] { (CommandRatings)Ranking }));
            }
            return roles;
        }

        public static async Task<List<AbbybotRole>> GetRolesFromUser(AbbybotUser u)
        {
            StringBuilder abisb = new StringBuilder();
            abisb.Clear();
            abisb.Append("SELECT * FROM `discord`.`roles` WHERE ");
            abisb.Append($"`UserId` = '{u.Id}'");
            abisb.Append($" && `GuildId` = '{u.userGuild.GuildId}';");

            List<AbbybotRole> roles = new List<AbbybotRole>();
            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            foreach (AbbyRow row in table)
            {
                long Roleid = (long)(row["RoleId"]);
                roles.AddRange(RoleManager.roles.Where(rx => rx.role == (ulong)Roleid).Select(rx => rx));
            }
            return roles;
        }

        public static async Task<bool> DoesRoleExist(ulong user, ulong guild, ulong role)
        {
            StringBuilder abisb = new StringBuilder();
            abisb.Append("SELECT * FROM `discord`.`roles` WHERE `UserId` = '");
            abisb.Append(user);
            abisb.Append("' && `Roleid` ='");
            abisb.Append(role);
            abisb.Append("' && `GuildId` ='");
            abisb.Append(guild);
            abisb.Append("';");

            var table = await AbbysqlClient.FetchSQL(abisb.ToString());

            return (table.Count > 0);
        }
    }
}