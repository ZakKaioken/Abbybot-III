using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Roles.sql;

using Capi.Interfaces;

using Discord.WebSocket;
using System;
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
            Console.WriteLine("getting ratings");
            foreach (var r in rolz)
            {
                
                Console.WriteLine($"role    {r.role}");
foreach (var rr in r.allowedRatings)
            {
                
                Console.WriteLine($"    rating    {rr}");

            }
            }
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
            Console.WriteLine($"Socket Guild user roles?!?! {sgu.Roles.Count}");
            Console.WriteLine($"Abbybot user roles?!?! {roles.Count}");
            foreach (SocketRole role in sgu.Roles)
            {
                Console.Write($"\n[{role.Name}]");
                foreach (AbbybotRole Role in roles)
                {
                    Console.Write($"-[d-{role.Name}-{Role.role}]-!");
                    if (role.Id == Role.role)
                    {
                        Console.Write("<- Added!!...");
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