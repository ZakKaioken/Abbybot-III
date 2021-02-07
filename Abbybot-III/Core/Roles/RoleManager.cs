using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Roles.sql;

using Capi.Interfaces;

using Discord.WebSocket;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Mysql
{
    class RoleManager
    {
        public static List<AbbybotRole> roles = new List<AbbybotRole>();

        public static async Task GetRoles(SocketGuild g)
        {
            roles.Clear();
            roles.AddRange(await RoleSql.GetRoles(g));
        }

        public static async Task<List<CommandRatings>> GetRatings(List<AbbybotRole> rolz)
        {
            List<CommandRatings> cmdrts = new List<CommandRatings>();
            foreach (var rol in rolz)
                cmdrts.AddRange(rol.allowedRatings);
            if (!cmdrts.Contains(CommandRatings.cutie))
                cmdrts.Add(CommandRatings.cutie);
            var e = cmdrts.GroupBy(o => o).Select(o => o.FirstOrDefault()).ToList();
            return await Task.FromResult(e);
        }

        public static async Task<List<AbbybotRole>> GetUserRoles(SocketGuildUser sgu)
        {
            await GetRoles(sgu.Guild);
            var rolez = new List<AbbybotRole>();
            foreach (SocketRole role in sgu.Roles)
            {
                foreach (AbbybotRole Role in roles)
                {
                    if (role.Id == Role.role)
                    {
                        rolez.Add(Role);
                        await Task.FromResult(Role);
                        await RoleSql.SetRole(sgu.Id, sgu.Guild.Id, role.Id);
                    }
                }
            }

            return rolez;
        }
    }
}