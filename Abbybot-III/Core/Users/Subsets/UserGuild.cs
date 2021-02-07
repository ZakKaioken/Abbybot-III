using System.Collections.Generic;

namespace Abbybot_III.Core.Data.User.Subsets
{
    public class UserGuild
    {
        public ulong GuildId;
        public string Username;
        public bool admin;
        public List<AbbybotRole> Roles;
    }
}