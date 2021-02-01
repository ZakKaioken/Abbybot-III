using System;
using System.Collections.Generic;
using System.Text;

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
